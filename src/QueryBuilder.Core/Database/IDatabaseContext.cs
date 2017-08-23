namespace QueryBuilder.Core.Database
{
    public interface IDatabaseContext<TConnection, TTransaction>
        where TConnection  : class
        where TTransaction : class
    {
        // TODO move it elsewhere
        TTransaction BeginTransaction();

        TConnection GetConnection();
    }
}
