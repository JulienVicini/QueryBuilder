using QueryBuilder.Core.Mappings;
using QueryBuilder.EFCore.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QueryBuilder.SqlServer;
using System;

namespace QueryBuilder.EFCore.SqlServer.Factories
{
    public class MappingAdapterFactory<T>
        : IMappingAdapterFactory<T>
        where T : class
    {
        private readonly DbContext _dbContext;

        public MappingAdapterFactory(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IMappingAdapter<T> Create()
        {
            IModel model = _dbContext.Model;

            return new EFCoreMappingAdapter<T>(
                model.FindEntityType(typeof(T))
            );
        }
    }
}
