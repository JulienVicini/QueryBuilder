using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using QueryBuilder.Core.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace QueryBuilder.EFCore.Database
{
    public class EFCoreContextAdapter<TConnection, TTransaction>
        : IDatabaseContext<TConnection, TTransaction>, ICommandProcessing
        where TConnection : DbConnection
        where TTransaction : DbTransaction
    {
        private readonly DatabaseFacade _dbFacade;

        public EFCoreContextAdapter(DatabaseFacade dbFacade)
        {
            _dbFacade = dbFacade ?? throw new ArgumentNullException(nameof(dbFacade));
        }

        public TTransaction BeginTransaction() => (TTransaction) _dbFacade.BeginTransaction().GetDbTransaction();

        public int ExecuteCommand(string query, IEnumerable<object> parameters) =>  _dbFacade.ExecuteSqlCommand(query, parameters);

        public TConnection GetConnection() => _dbFacade.GetDbConnection() as TConnection;
    }
}
