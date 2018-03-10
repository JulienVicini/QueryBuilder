using QueryBuilder.Core.Mappings;
using QueryBuilder.EFCore.Mappings;
using QueryBuilder.EFCore.SqlServer.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace QueryBuilder.EFCore.SqlServer.Factories
{
    public class MappingAdapterFactory<T>
        where T : class
    {

        public IMappingAdapter<T> CreateMappingAdapter(IQueryable<T> queryable)
        {
            DbContext dbContext = IQueryableHelper.GetDbContext(queryable);

            IModel model = dbContext.Model;

            return new EFCoreMappingAdapter<T>(
                model.FindEntityType(typeof(T))
            );
        }
    }
}
