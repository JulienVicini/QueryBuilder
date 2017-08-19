using EntityFramework.Extensions.Core.Queries.Statements.Values;

namespace EntityFramework.Extensions.Core.Queries.Statements.Operators
{
    public class AssignStatement<TEntity>
        where TEntity : class
    {
        public ColumnValueStatement<TEntity> Column { get; private set; }

        public IValueStatement Value { get; private set; }

        public AssignStatement(ColumnValueStatement<TEntity> column, IValueStatement value)
        {
            Column = column;
            Value  = value;
        }
    }
}
