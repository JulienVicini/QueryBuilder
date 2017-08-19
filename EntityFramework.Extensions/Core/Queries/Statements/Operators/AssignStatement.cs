using EntityFramework.Extensions.Core.Mappings;

namespace EntityFramework.Extensions.Core.Queries.Statements.Operators
{
    public class AssignStatement<TEntity>
        where TEntity : class
    {
        public ColumnMapping<TEntity> Column { get; private set; }

        public IValueStatement Value { get; private set; }

        public AssignStatement(ColumnMapping<TEntity> column, IValueStatement value)
        {
            Column = column;
            Value  = value;
        }
    }
}
