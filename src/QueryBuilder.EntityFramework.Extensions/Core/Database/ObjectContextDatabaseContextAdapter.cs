using QueryBuilder.Core.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace QueryBuilder.EntityFramework.Extensions.Core.Database
{
    public class ObjectContextDatabaseContextAdapter
        : IDatabaseContext, ICommandProcessing
    {
        private readonly ObjectContext _objectContext;

        public ObjectContextDatabaseContextAdapter(ObjectContext objectContext)
        {
            _objectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        public DbTransaction BeginTransaction()
        {
            return _objectContext.Connection.BeginTransaction();
        }

        public DbConnection GetConnection()
        {
            return _objectContext.Connection;
        }

        public int ExecuteCommand(string query, IEnumerable<object> parameters)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _objectContext.ExecuteStoreCommand( query, parameters.ToArray() );
        }
    }
}
