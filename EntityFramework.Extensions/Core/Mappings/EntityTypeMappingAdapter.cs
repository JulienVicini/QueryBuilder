using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace EntityFramework.Extensions.Core.Mappings
{
    public class EntityTypeMappingAdapter<TEntity>
        : IMappingAdapter<TEntity>
        where TEntity : class
    {
        private readonly EntityType _entityType;

        public EntityTypeMappingAdapter(EntityType entityType)
        {
            _entityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }

        public Type EntityType => typeof(TEntity);

        public string GetTableName()
        {
            MetadataProperty metadata = null;

            if( _entityType.MetadataProperties.TryGetValue("TableName", false, out metadata) && metadata.Value != null )
            {
                return "[" + metadata.Value.ToString().Replace(".", "].[") + "]";
            }
            else
                return $"[dbo].[{EntityType.Name}]";
        }

        public IEnumerable<ColumnMapping<TEntity>> GetColumns()
        {
            foreach (EdmProperty edmProperty in _entityType.DeclaredProperties)
                yield return Map(edmProperty);
        }

        public ColumnMapping<TEntity> Map(EdmProperty edmProperty)
        {
            string propertyName = (string)edmProperty.MetadataProperties["PreferredName"].Value;

            return new ColumnMapping<TEntity>(
                dbColumnName      : edmProperty.Name,
                isIdentity  : edmProperty.IsStoreGeneratedIdentity,
                propertyInfo: typeof(TEntity).GetProperty(propertyName)
            );
        }
    }
}
