using System;
using System.Reflection;

namespace EntityFramework.Extensions.Core.Mappings
{
    public class ColumnMapping<TEntity>
        where TEntity : class
    {
        public string DbColumnName { get; private set; }

        public bool IsIdentity { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public ColumnMapping(string dbColumnName, bool isIdentity, PropertyInfo propertyInfo)
        {
            DbColumnName = dbColumnName ?? throw new ArgumentNullException(nameof(dbColumnName));
            IsIdentity   = isIdentity;
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
    }
}
