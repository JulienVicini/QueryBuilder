using QueryBuilder.Core.Helpers;

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

        public AlterType Type { get; }

        public string TableName { get; }

        public AlterTableStatement(string tableName, AlterType type)
        {
            Check.NotNullOrEmpty(tableName, nameof(tableName));

            TableName = tableName;
            Type      = type;
        }
    }
}
