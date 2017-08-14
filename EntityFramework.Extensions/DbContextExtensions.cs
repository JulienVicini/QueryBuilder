using EntityFramework.Extensions.BulkCopy;
using EntityFramework.Extensions.Mappings;
using EntityFramework.Extensions.QueriesHelpers;
using EntityFramework.Extensions.Sql;
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

        // TODO refactor this
        public static TableMapping GetTableMapping<TEntity>( this ObjectContext objectContext)
            where TEntity : class
        {
            EntityType entityType =  objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.SSpace)
                                                                    .First(e => e.Name == typeof(TEntity).Name);

            var sets = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
            // Mappings
            IEnumerable<ColumnMapping> mappings
                = entityType.DeclaredProperties.Select(edm => ColumnMapping.FromMetaDdata(edm))
                                               .ToList();

            // Table Name
            MetadataProperty metaData = entityType.MetadataProperties.FirstOrDefault(m => m.Name == "Configuration");

            string tableName = typeof(TEntity).Name, schema = "dbo";

            if(metaData != null)
            {
                //entityType.Entity.GetTableName();
                //var config = (System.Data.Entity.ModelConfiguration.Configuration.Mapping.EntityMappingConfiguration)metaData.Value;
                //tableName = config
            }

            return new TableMapping(
                tableName: tableName,
                schema   : schema,
                columns  : mappings
            );
        }

        public static void BulkInsert<T>(this DbSet<T> dbSet, IEnumerable<T> data)
            where T : class
        {
            ObjectContext context = IQueryableHelpers.GetObjectContext(dbSet);

            ISqlContext sqlContext = new SqlContext(context);
            IDataTableFactory<T> dataTableFactory = new DataTableFactory<T>();

            var bulkCopy = new SqlBulkCopyProcessor<T>(
                dataTableFactory: dataTableFactory,
                sqlContext      : sqlContext,
                tableMapping    : context.GetTableMapping<T>()
            );

            bulkCopy.Execute(data);
        }

        //public static GlobalItem GetEntityMetadata<T>( this DbContext dbContext )
        //{
        //    return dbContext.GetEntitiesMetadata()
        //                    .First(item => item.)
        //}
    }
}
