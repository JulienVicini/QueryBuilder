namespace EntityFramework.Extensions.Core.Queries
{
    public class AlterTableQuery<TEntity>
        where TEntity : class
    {
        public enum AlterType
        {
            Create,
            Drop
        }

        public AlterType Type { get; private set; }

        public string TableName { get; private set; }

        public AlterTableQuery(string tableName, AlterType type)
        {
            TableName = tableName;
            Type      = type;
        }
    }
}
