﻿using QueryBuilder.Core.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace QueryBuilder.EntityFramework.Database
{
    public class ObjectContextDatabaseAdapter<TConnection, TTransaction>
        : IDatabaseContext<TConnection, TTransaction>, ICommandProcessing
        where TConnection  : DbConnection
        where TTransaction : DbTransaction
    {
        private readonly ObjectContext _objectContext;

        public ObjectContextDatabaseAdapter(ObjectContext objectContext)
        {
            _objectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        public ITransactionScope<TTransaction> BeginTransaction()
        {
            DbTransaction dbTransaction = _objectContext.Connection.BeginTransaction();

            return new DbTransactionTransactionScope<TTransaction>(dbTransaction);
        }

        public TConnection GetConnection()
        {
            return ((EntityConnection)_objectContext.Connection).StoreConnection as TConnection;
        }

        public int ExecuteCommand(string query, IEnumerable<object> parameters)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return _objectContext.ExecuteStoreCommand( query, parameters.ToArray() );
        }
    }
}
