using EntityFramework.Extensions.Core.BulkCopy;
using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Core.Queries;
using EntityFramework.Extensions.QueriesHelpers;
using EntityFramework.Extensions.SqlServer;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;

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
            var bulkCopy = new BulkCopy<T, DataTable>(
                bulkCopyExecutor: new SqlBulkCopyExecutor(databaseContext),
                dataTransformer : new DataTableDataTransformer<T>(mappingAdapter),
                mappingAdapter  : mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }
    }
}
