namespace EntityFramework.Extensions.Core.BulkCopy
{
    public interface IBulkCopyExecutor<T>
        where T : class
    {
        void Write(string tableName, T records);
    }
}
