using QueryBuilder.Core.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace QueryBuilder.EntityFramework.Database
{
    public class EFCoreContextAdapter<TConnection, TTransaction>
        : IDatabaseContext<TConnection, TTransaction>, ICommandProcessing
        where TConnection : DbConnection
        where TTransaction : DbTransaction
    {
        public TTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public int ExecuteCommand(string query, IEnumerable<object> parameters)
        {
            throw new NotImplementedException();
        }

        public TConnection GetConnection()
        {
            throw new NotImplementedException();
        }
    }
}
