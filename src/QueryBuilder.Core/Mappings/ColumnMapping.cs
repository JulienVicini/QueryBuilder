using QueryBuilder.Core.Helpers;
using System;
using System.Data;
using System.Reflection;

namespace QueryBuilder.Core.Mappings
{
    public class ColumnMapping<TEntity>
        where TEntity : class
    {
        public string DbColumnName { get; }

        public DbType DbType { get; }

        public bool IsIdentity { get; }

        public int? Length { get; }

        public int? Precision { get; }

        public int? Scale { get; }

        public bool IsRequired { get; }

        public PropertyInfo PropertyInfo { get; }

        public ColumnMapping(string dbColumnName, DbType dbType, bool isIdentity, int? length, int? precision, int? scale, bool isRequired, PropertyInfo propertyInfo)
        {
            Check.NotNullOrWhiteSpace(dbColumnName, nameof(dbColumnName));

            DbColumnName = dbColumnName;
            DbType       = dbType;
            IsIdentity   = isIdentity;
            Length       = length;
            Precision    = precision;
            Scale        = scale;
            IsRequired   = isRequired;
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

            if (propertyInfo.DeclaringType != typeof(TEntity))
                throw new InvalidOperationException($"The property info \"{nameof(propertyInfo)}\" declarating type must be \"{typeof(TEntity)}\".");
        }
    }
}
