using QueryBuilder.EntityFramework.Helpers;
using QueryBuilder.EntityFramework.Mappings;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace QueryBuilder.EntityFramework.SqlServer.Factories
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
