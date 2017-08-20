using EntityFramework.Extensions.Core.Bulk;
using EntityFramework.Extensions.Core.Database;
using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Core.Queries;
using EntityFramework.Extensions.Helpers;
using EntityFramework.Extensions.SqlServer;
using EntityFramework.Extensions.SqlServer.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions
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

            IDatabaseContext databaseContext = new ObjectContextDatabaseContextAdapter(context);

            IMappingAdapter<T> mappingAdapter = new EntityTypeMappingAdapter<T>(
                context.GetEntityMetaData<T>()
            );

            // TODO put in factory
            var bulkCopy = new BulkOrchestrator<T, DataTable>(
                bulkExecutor   : new SqlBulkCopyExecutor(databaseContext),
                dataTransformer: new DataTableDataTransformer<T>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }

        public static void BulkMerge<TEntity, TColumns>( this DbSet<TEntity> dbSet, IEnumerable<TEntity> data, Expression<Func<TEntity, TColumns>> mergeColumns, bool updateOnly = false)
            where TEntity : class
        {
            throw new NotImplementedException();
        }

        public static int Delete<T>(this IQueryable<T> queryable)
            where T : class
        {
            // BuilderQuery
            var query = new DeleteQuery<T>(IQueryableHelpers.GetQueryPredicate(queryable));

            // Get info from IQueryable
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            var objectContextAdapter = new ObjectContextDatabaseContextAdapter(objectContext);

            var mappingAdapter = new EntityTypeMappingAdapter<T>(objectContext.GetEntityMetaData<T>());

            // Create Query
            var orchestrator = new QueryOrchestrator<T>(
                new SqlQueryTranslator<T>(mappingAdapter),
                objectContextAdapter
            );

            return orchestrator.Delete(query);
        }

        public static void Update<T>(this IQueryable<T> query, params Expression<Action<T>>[] setters)
            where T : class => throw new NotImplementedException();

        //public static int ProcessQuery(Query command, ObjectContext objectContext)
        //{
        //    // Execute
        //    ICommandProcessing queryProcessor = new ObjectContextDatabaseContextAdapter(objectContext);
        //    var processor = new QueryProcessor(queryProcessor);

        //    return processor.ExecuteQuery(command);
        //}
    }
}
