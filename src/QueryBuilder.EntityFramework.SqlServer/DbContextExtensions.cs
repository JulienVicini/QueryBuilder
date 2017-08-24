﻿using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Queries;
using QueryBuilder.EntityFramework.Database;
using QueryBuilder.EntityFramework.IQueryable;
using QueryBuilder.EntityFramework.Mappings;
using QueryBuilder.Helpers;
using QueryBuilder.Queries;
using QueryBuilder.SqlServer.Bulk;
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
    public static class DbContextExtensions
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
            var bulkCopy = new BulkCoordinator<T, DataTable>(
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
            var mergeQuery = new MergeQuery<TEntity>(mergeKeys, temporaryTable, updateOnly ? MergeQuery<TEntity>.MergeType.UpdateOnly : MergeQuery<TEntity>.MergeType.InsertOrUpdate);
            var queryOrchestrator = new QueryCoordinator<TEntity>(
                new SqlQueryTranslator<TEntity>(mappingAdapter),
                databaseContext
            ); 

            // TODO put in factory
            var bulkCopy = new BulkCoordinator<TEntity, DataTable>(
                bulkExecutor   : new SqlBulkMergeExecutor<TEntity>(databaseContext, queryOrchestrator, mergeQuery),
                dataTransformer: new DataTableDataTransformer<TEntity>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }

        public static int Delete<T>(this IQueryable<T> queryable)
            where T : class
        {
            // BuilderQuery
            var query = new DeleteQuery<T>(IQueryableHelpers.GetQueryPredicate(queryable));

            // Get info from IQueryable
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            var objectContextAdapter = new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(objectContext);

            var mappingAdapter = new EntityTypeMappingAdapter<T>(objectContext.GetEntityMetaData<T>());

            // Create Query
            var orchestrator = new QueryCoordinator<T>(
                new SqlQueryTranslator<T>(mappingAdapter),
                objectContextAdapter
            );

            return orchestrator.Delete(query);
        }

        public static UpdateQueryBuilder<T> SetValue<T, TValue>( this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, Expression<Func<T, TValue>> valueExpression )
        {
            return new UpdateQueryBuilder<T>(queryable)
                        .SetValue(memberExpression, valueExpression);
        }

        public static UpdateQueryBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, TValue value)
        {
            return new UpdateQueryBuilder<T>(queryable)
                        .SetValue(memberExpression, value);
        }

        public static int Update<T>(this UpdateQueryBuilder<T> updateBuilder) where T : class
        {
            IQueryable<T> queryable = updateBuilder.Queryable;

            // Create Query
            var query = new UpdateQuery<T>(
                updateBuilder.Assignements,
                IQueryableHelpers.GetQueryPredicate(queryable)
            );

            // Get info from IQueryable
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            var objectContextAdapter = new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(objectContext);

            var mappingAdapter = new EntityTypeMappingAdapter<T>(objectContext.GetEntityMetaData<T>());

            // Create Query
            var orchestrator = new QueryCoordinator<T>(
                new SqlQueryTranslator<T>(mappingAdapter),
                objectContextAdapter
            );

            return orchestrator.Update(query);
        }

        //public static int ProcessQuery(Query command, ObjectContext objectContext)
        //{
        //    // Execute
        //    ICommandProcessing queryProcessor = new ObjectContextDatabaseContextAdapter(objectContext);
        //    var processor = new QueryProcessor(queryProcessor);

        //    return processor.ExecuteQuery(command);
        //}
    }
}