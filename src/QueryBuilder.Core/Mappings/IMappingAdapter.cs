using System;
using System.Collections.Generic;

namespace QueryBuilder.Core.Mappings
{
    public interface IMappingAdapter<TEntity>
        where TEntity : class
    {
        Type EntityType { get; }

        string GetTableName();

        IEnumerable<ColumnMapping<TEntity>> GetColumns();
    }
}
