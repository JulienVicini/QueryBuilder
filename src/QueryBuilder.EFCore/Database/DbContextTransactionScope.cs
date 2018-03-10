using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using QueryBuilder.Core.Database;

namespace QueryBuilder.EFCore.Database
{
    public class DbContextTransactionScope<TTransaction>
        : ITransactionScope<TTransaction>
        where TTransaction : DbTransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;
        public TTransaction Current => (TTransaction)_dbContextTransaction.GetDbTransaction();

        public DbContextTransactionScope(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction ?? throw new ArgumentNullException(nameof(dbContextTransaction));
        }

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
        }
    }
}
