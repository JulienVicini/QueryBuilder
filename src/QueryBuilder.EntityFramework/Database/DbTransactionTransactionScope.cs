using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Core.Database;

namespace QueryBuilder.EntityFramework.Database
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
