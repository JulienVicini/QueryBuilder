using System;
using QueryBuilder.Core.Mappings;

namespace QueryBuilder.SqlServer
{
    public interface IMappingAdapterFactory<TEntity>
        where TEntity : class
    {
        IMappingAdapter<TEntity> Create();
    }
}
