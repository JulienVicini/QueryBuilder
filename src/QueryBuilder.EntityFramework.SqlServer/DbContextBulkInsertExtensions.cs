using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Statements;
using QueryBuilder.EntityFramework.Database;
using QueryBuilder.EntityFramework.IQueryable;
using QueryBuilder.EntityFramework.Mappings;
using QueryBuilder.Helpers;
using QueryBuilder.SqlServer.Bulk;
using QueryBuilder.SqlServer.Statements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.SqlServer
{
    public static class DbContextBulkInsertExtensions
    {
        public static EntityType GetEntityMetaData<TEntity>(this ObjectContext objectContext)
        {
            string entityFullName = typeof(TEntity).Name;

            return objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.SSpace)
                                                  .First(e => e.Name == entityFullName);
        }

        public static void BulkInsert<T>(this DbSet<T> dbSet, IEnumerable<T> data)
            where T : class
        {
            ObjectContext context = IQueryableHelpers.GetObjectContext(dbSet);

            IDatabaseContext<SqlConnection, SqlTransaction> databaseContext = new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(context);

            IMappingAdapter<T> mappingAdapter = new EntityTypeMappingAdapter<T>(
                context.GetEntityMetaData<T>()
            );

            // TODO put in factory
            var bulkCopy = new BulkFacade<T, DataTable>(
                bulkExecutor   : new SqlBulkCopyExecutor(databaseContext),
                dataTransformer: new DataTableDataTransformer<T>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }

        public static void BulkMerge<TEntity, TColumns>( this DbSet<TEntity> dbSet, IEnumerable<TEntity> data, Expression<Func<TEntity, TColumns>> mergeOnColumns, bool updateOnly = false)
            where TEntity : class
        {
            ObjectContext context = IQueryableHelpers.GetObjectContext(dbSet);

            var databaseContext = new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(context);

            IMappingAdapter<TEntity> mappingAdapter = new EntityTypeMappingAdapter<TEntity>(
                context.GetEntityMetaData<TEntity>()
            );

            IEnumerable<MemberExpression> mergeKeys = ExpressionHelper.GetSelectedMemberInAnonymousType(mergeOnColumns);

            string temporaryTable = "#tmp_bulk";

            // TODO change update or insert selection
            var mergeQuery = new MergeStatement<TEntity>(mergeKeys, temporaryTable, updateOnly ? MergeStatement<TEntity>.MergeType.UpdateOnly : MergeStatement<TEntity>.MergeType.InsertOrUpdate);
            var queryOrchestrator = new StatementFacade<TEntity>(
                new SqlQueryTranslator<TEntity>(mappingAdapter),
                databaseContext
            ); 

            // TODO put in factory
            var bulkCopy = new BulkFacade<TEntity, DataTable>(
                bulkExecutor   : new SqlBulkMergeExecutor<TEntity>(databaseContext, queryOrchestrator, mergeQuery),
                dataTransformer: new DataTableDataTransformer<TEntity>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }
    }
}
