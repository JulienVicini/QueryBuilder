using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static IEnumerable<EntityType> GetEntitiesMetadata(this DbContext dbContext )
        {
            ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;

            return objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.SSpace);
        }

        public static EntityType GetEntityMetadata<TEntity>( this DbContext dbContext )
        {
            string entityFullName = typeof(TEntity).Name;

            return dbContext.GetEntitiesMetadata()
                            .First(e => e.Name == entityFullName);
        }

        public static EdmProperty GetPropertyMetaData<TEntity, TProperty>( this DbContext dbContext, Expression<Func<TEntity, TProperty>> expression )
            where TEntity : class
        {
            EntityType entityType = dbContext.GetEntityMetadata<TEntity>();

            string propertyName = ExpressionHelper.GetMemberName(expression);

            return entityType.DeclaredProperties.First(p => p.MetadataProperties.Any(m => m.Name == "PreferredName" && m.Value as string == propertyName));
        }

        public static void BulkInsert<T>(this DbSet<T> dbSet, IEnumerable<T> data)
            where T : class
        {
            throw new NotImplementedException();
        }

        //public static GlobalItem GetEntityMetadata<T>( this DbContext dbContext )
        //{
        //    return dbContext.GetEntitiesMetadata()
        //                    .First(item => item.)
        //}
    }
}
