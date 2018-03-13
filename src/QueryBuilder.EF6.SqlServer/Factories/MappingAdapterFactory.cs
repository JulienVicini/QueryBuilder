using QueryBuilder.Core.Mappings;
using QueryBuilder.EF6.Mappings;
using QueryBuilder.SqlServer;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;

namespace QueryBuilder.EF6.SqlServer.Factories
{
    public class MappingAdapterFactory<T>
        : IMappingAdapterFactory<T>
        where T : class
    {
        private readonly ObjectContext _objectContext;

        public MappingAdapterFactory(ObjectContext objectContext)
        {
            _objectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        public IMappingAdapter<T> Create()
        {
            EntityType entityType = _objectContext.GetEntityMetaData<T>();

            return new EntityTypeMappingAdapter<T>(entityType);
        }
    }
}
