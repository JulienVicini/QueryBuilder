using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;

namespace QueryBuilder.EntityFramework.Extensions.Core.Mappings
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
                dbColumnName: edmProperty.Name,
                dbType      : ConvertPrimitiveTypeKind( edmProperty.PrimitiveType.PrimitiveTypeKind, edmProperty.IsFixedLength, edmProperty.IsUnicode ),
                isIdentity  : edmProperty.IsStoreGeneratedIdentity,
                isRequired  : !edmProperty.Nullable,
                length      : edmProperty.MaxLength,
                precision   : edmProperty.Precision,
                scale       : edmProperty.Scale,
                propertyInfo: typeof(TEntity).GetProperty(propertyName)
            );
        }

        public static DbType ConvertPrimitiveTypeKind(PrimitiveTypeKind typeKind, bool? isFixedLength, bool? isUnicode)
        {
            switch (typeKind)
            {
                case PrimitiveTypeKind.Binary  : return DbType.Binary;
                case PrimitiveTypeKind.Boolean : return DbType.Boolean;
                case PrimitiveTypeKind.Byte    : return DbType.Byte;
                case PrimitiveTypeKind.DateTime: return DbType.DateTime;
                case PrimitiveTypeKind.Decimal : return DbType.Decimal;
                case PrimitiveTypeKind.Double  : return DbType.Double;
                case PrimitiveTypeKind.Guid    : return DbType.Guid;
                case PrimitiveTypeKind.Single  : return DbType.Single;
                case PrimitiveTypeKind.SByte   : return DbType.SByte;
                case PrimitiveTypeKind.Int16   : return DbType.Int16;
                case PrimitiveTypeKind.Int32   : return DbType.Int32;
                case PrimitiveTypeKind.Int64   : return DbType.Int64;
                case PrimitiveTypeKind.String:
                    if (isFixedLength.Value)
                        return isUnicode.Value
                            ? DbType.StringFixedLength
                            : DbType.AnsiStringFixedLength;
                    else
                        return isUnicode.Value
                            ? DbType.String
                            : DbType.AnsiString;
                case PrimitiveTypeKind.Time          : return DbType.Time;
                case PrimitiveTypeKind.DateTimeOffset: return DbType.DateTimeOffset;

                // Not Implemeted
                case PrimitiveTypeKind.Geometry:
                case PrimitiveTypeKind.Geography:
                case PrimitiveTypeKind.GeometryPoint:
                case PrimitiveTypeKind.GeometryLineString:
                case PrimitiveTypeKind.GeometryPolygon:
                case PrimitiveTypeKind.GeometryMultiPoint:
                case PrimitiveTypeKind.GeometryMultiLineString:
                case PrimitiveTypeKind.GeometryMultiPolygon:
                case PrimitiveTypeKind.GeometryCollection:
                case PrimitiveTypeKind.GeographyPoint:
                case PrimitiveTypeKind.GeographyLineString:
                case PrimitiveTypeKind.GeographyPolygon:
                case PrimitiveTypeKind.GeographyMultiPoint:
                case PrimitiveTypeKind.GeographyMultiLineString:
                case PrimitiveTypeKind.GeographyMultiPolygon:
                case PrimitiveTypeKind.GeographyCollection:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
