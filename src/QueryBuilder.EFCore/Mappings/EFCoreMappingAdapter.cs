using QueryBuilder.Core.Mappings;
using System;
using System.Collections.Generic;

namespace QueryBuilder.EntityFramework.Mappings
{
    public class EFCoreMappingAdapter<TEntity>
        : IMappingAdapter<TEntity>
        where TEntity : class
    {
        public Type EntityType => throw new NotImplementedException();

        public IEnumerable<ColumnMapping<TEntity>> GetColumns()
        {
            throw new NotImplementedException();
        }

        public string GetTableName()
        {
            throw new NotImplementedException();
        }
    }
}
