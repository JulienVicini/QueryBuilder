using System;
using System.Data;
using System.Reflection;

namespace QueryBuilder.EntityFramework.Extensions.Core.Mappings
{
    public class ColumnMapping<TEntity>
        where TEntity : class
    {
        public string DbColumnName { get; private set; }

        public DbType DbType { get; private set; }

        public bool IsIdentity { get; private set; }

        public int? Length { get; private set; }

        public int? Precision { get; private set; }

        public int? Scale { get; private set; }

        public bool IsRequired { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public ColumnMapping(string dbColumnName, DbType dbType, bool isIdentity, int? length, int? precision, int? scale, bool isRequired, PropertyInfo propertyInfo)
        {
            DbColumnName = dbColumnName ?? throw new ArgumentNullException(nameof(dbColumnName));
            DbType       = dbType;
            IsIdentity   = isIdentity;
            Length       = length;
            Precision    = precision;
            Scale        = scale;
            IsRequired   = isRequired;
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
    }
}
