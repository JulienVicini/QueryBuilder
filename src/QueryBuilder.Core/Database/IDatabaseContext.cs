namespace QueryBuilder.Core.Database
{
    public interface IDatabaseContext<TConnection, TTransaction>
        where TConnection  : class
        where TTransaction : class
    {
        ITransactionScope<TTransaction> BeginTransaction();

        TConnection GetConnection();
    }
}
