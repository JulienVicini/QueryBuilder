namespace QueryBuilder.Core.Statements
{
    public class AlterTableStatement<TEntity>
        where TEntity : class
    {
        public enum AlterType
        {
            Create,
            Drop
        }

        public AlterType Type { get; private set; }

        public string TableName { get; private set; }

        public AlterTableStatement(string tableName, AlterType type)
        {
            TableName = tableName;
            Type      = type;
        }
    }
}
