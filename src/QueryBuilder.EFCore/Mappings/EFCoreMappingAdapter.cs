using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QueryBuilder.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.EFCore.Mappings
{
    public class EFCoreMappingAdapter<TEntity>
        : IMappingAdapter<TEntity>
        where TEntity : class
    {
        public Type EntityType => typeof(TEntity);

        private readonly IEntityType _entityType;
        private readonly RelationalSqlTypePropertyTranslator _relationSqlTypeTranslator;

        public EFCoreMappingAdapter(IEntityType entityType) : this(entityType, new RelationalSqlTypePropertyTranslator()) { }
        public EFCoreMappingAdapter(IEntityType entityType, RelationalSqlTypePropertyTranslator relationSqlTypeTranslator)
        {
            _entityType                = entityType                ?? throw new ArgumentNullException(nameof(entityType));
            _relationSqlTypeTranslator = relationSqlTypeTranslator ?? throw new ArgumentNullException(nameof(relationSqlTypeTranslator));
        }

        public IEnumerable<ColumnMapping<TEntity>> GetColumns()
        {
            foreach (IProperty property in _entityType.GetProperties())
            {
                IRelationalPropertyAnnotations relationalProperty = property.Relational();

                bool isKey = _entityType.FindPrimaryKey().Properties.Contains(property);

                _relationSqlTypeTranslator.GetMetaData(relationalProperty.ColumnType, out int? precision, out int? scale, out System.Data.DbType dbType);

                yield return new ColumnMapping<TEntity>(
                    dbColumnName: relationalProperty.ColumnName,
                    dbType      : dbType,
                    isIdentity  : property.ValueGenerated == ValueGenerated.OnAdd,
                    propertyInfo: typeof(TEntity).GetProperty(property.Name),
                    precision   : precision,
                    isRequired  : !property.IsNullable,
                    length      : property.GetMaxLength(),
                    scale       : scale
                );
            }
        }

        public string GetTableName()
        {
            IRelationalEntityTypeAnnotations relationalEntityType = _entityType.Relational();

            return $"[{relationalEntityType.Schema}].[{relationalEntityType.TableName}]";
        }
    }
}
