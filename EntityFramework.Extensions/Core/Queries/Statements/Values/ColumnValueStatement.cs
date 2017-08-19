using EntityFramework.Extensions.Core.Mappings;

namespace EntityFramework.Extensions.Core.Queries.Statements.Values
{
    public class ColumnValueStatement<TEntity> 
        : IValueStatement
        where TEntity : class
    {
        public ColumnMapping<TEntity> ColumnMapping { get; private set; }

        public ColumnValueStatement(ColumnMapping<TEntity> mapping)
        {
            ColumnMapping = mapping;
        }

        public void Visit(IStatementTranslator queryTranslator)
        {
            queryTranslator.TranslateColumnExpressionStatement(this);
        }
    }
}
