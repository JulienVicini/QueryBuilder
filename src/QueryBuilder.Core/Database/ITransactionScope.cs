using System;

namespace QueryBuilder.Core.Database
{
    public interface ITransactionScope<TTransaction> : IDisposable
    {
        TTransaction Current { get; }

        void Commit();
    }
}
