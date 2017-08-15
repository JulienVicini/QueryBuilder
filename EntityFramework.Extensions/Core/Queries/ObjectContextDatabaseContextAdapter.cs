using System;
using System.Data.Common;
using System.Data.Entity.Core.Objects;

namespace EntityFramework.Extensions.Core.Queries
{
    public class ObjectContextDatabaseContextAdapter
        : IDatabaseContext
    {
        private readonly ObjectContext _objectContext;

        public ObjectContextDatabaseContextAdapter(ObjectContext objectContext)
        {
            _objectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        public DbConnection GetConnection()
        {
            return _objectContext.Connection;
        }
    }
}
