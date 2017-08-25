using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Statements;
using QueryBuilder.EntityFramework.SqlServer.Factories;
using QueryBuilder.Helpers;
using QueryBuilder.SqlServer.Bulk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
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
            BulkFacade<T, DataTable> bulkCopy
                = new BulkFacadeFactory<T>().CreateBulkCopy(dbSet);

            bulkCopy.WriteToServer(data);
        }

        public static void BulkMerge<TEntity, TColumns>( this DbSet<TEntity> dbSet, IEnumerable<TEntity> data, Expression<Func<TEntity, TColumns>> mergeOnColumns, bool updateOnly = false)
            where TEntity : class
        {
            var databaseContext = new DatabaseAdapterFactory<TEntity>().CreateDatabaseContext(dbSet);
            IMappingAdapter<TEntity> mappingAdapter = new MappingAdapterFactory<TEntity>().CreateMappingAdapter(dbSet);

            IEnumerable<MemberExpression> mergeKeys = ExpressionHelper.GetSelectedMemberInAnonymousType(mergeOnColumns);

            string temporaryTable = "#tmp_bulk";

            // TODO change update or insert selection
            var mergeQuery = new MergeStatement<TEntity>(mergeKeys, temporaryTable, updateOnly ? MergeStatement<TEntity>.MergeType.UpdateOnly : MergeStatement<TEntity>.MergeType.InsertOrUpdate);
            var queryFacade = new StatementFacadeFactory<TEntity>().CreateFacade(dbSet); 

            // TODO put in factory
            var bulkCopy = new BulkFacade<TEntity, DataTable>(
                bulkExecutor   : new SqlBulkMergeExecutor<TEntity>(databaseContext, queryFacade, mergeQuery),
                dataTransformer: new DataTableDataTransformer<TEntity>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }
    }
}
