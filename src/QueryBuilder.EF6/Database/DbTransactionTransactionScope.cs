using System;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using QueryBuilder.Core.Database;

namespace QueryBuilder.EF6.Database
{
    public class DbTransactionTransactionScope<TTransaction>
        : ITransactionScope<TTransaction>
        where TTransaction : DbTransaction
    {
        private readonly DbTransaction _dbTransaction;
        public TTransaction Current =>  (TTransaction)((EntityTransaction)_dbTransaction).StoreTransaction;

        public DbTransactionTransactionScope(DbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
        }
    }
}
