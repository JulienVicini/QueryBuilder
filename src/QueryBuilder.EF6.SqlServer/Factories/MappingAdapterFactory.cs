using QueryBuilder.EF6.Helpers;
using QueryBuilder.EF6.Mappings;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace QueryBuilder.EF6.SqlServer.Factories
{
    public class MappingAdapterFactory<T>
        where T : class
    {

        public EntityTypeMappingAdapter<T> CreateMappingAdapter(IQueryable<T> queryable)
        {
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            return new EntityTypeMappingAdapter<T>(
                objectContext.GetEntityMetaData<T>()
            );
        }
    }
}
