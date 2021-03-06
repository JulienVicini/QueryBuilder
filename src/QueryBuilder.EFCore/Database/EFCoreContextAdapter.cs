﻿using Microsoft.EntityFrameworkCore;
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
        : IDatabaseContext<TConnection, TTransaction>, ICommandProcessing<TTransaction>
        where TConnection : DbConnection
        where TTransaction : DbTransaction
    {
        private readonly DatabaseFacade _dbFacade;

        public EFCoreContextAdapter(DatabaseFacade dbFacade)
        {
            _dbFacade = dbFacade ?? throw new ArgumentNullException(nameof(dbFacade));
        }

        public ITransactionScope<TTransaction> BeginTransaction() {
            IDbContextTransaction transaction = _dbFacade.CurrentTransaction 
                                                    ?? _dbFacade.BeginTransaction();

            return new DbContextTransactionScope<TTransaction>(transaction);
        }

        public int ExecuteCommand(string query, IEnumerable<object> parameters) =>  _dbFacade.ExecuteSqlCommand(query, parameters);
        public int ExecuteCommand(string query, IEnumerable<object> parameters, TTransaction transaction) => ExecuteCommand(query, parameters); // TODO ensure the transaciton behavior is correct

        public TConnection GetConnection() => _dbFacade.GetDbConnection() as TConnection;
    }
}
