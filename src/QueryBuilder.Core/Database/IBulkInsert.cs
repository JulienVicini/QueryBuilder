namespace QueryBuilder.Core.Database
{
    public interface IBulkInsert<T>
        where T : class
    {
        void Write(string tableName, T records);
    }
}
