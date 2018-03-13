using QueryBuilder.EF6.SqlServer.Factories;
using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Core.Services;

namespace QueryBuilder.EF6.SqlServer
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
            // Resolve Service
            var serviceFactory = new ServiceFactory<T>(dbSet);
            IBulkInsertService<T> bulkInsertService = serviceFactory.CreateBulkInsert();

            // Perform Bulk Insert
            bulkInsertService.WriteToServer(data);
        }

        public static void BulkMerge<TEntity, TColumns>( this DbSet<TEntity> dbSet, IEnumerable<TEntity> data, Expression<Func<TEntity, TColumns>> mergeOnColumns, bool updateOnly = false)
            where TEntity : class
        {
            // Resolve service
            var serviceFactory = new ServiceFactory<TEntity>(dbSet);
            IBulkMergeService<TEntity> bulkMergeService = serviceFactory.CreateBulkMerge();

            // Perform Bulk Merge
            IEnumerable<MemberExpression> mergeKeys 
                = ExpressionHelper.GetSelectedMemberInAnonymousType(mergeOnColumns); // TODO move this logic inside the service

            bulkMergeService.WriteToServer(data, mergeKeys, updateOnly);
        }
    }
}
