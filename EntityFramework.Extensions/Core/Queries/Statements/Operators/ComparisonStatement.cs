namespace EntityFramework.Extensions.Core.Queries.Statements.Operators
{
    public class ComparisonStatement
        : IFilterStatement
    {
        public IValueStatement Left { get; private set; }

        public ComparisonOperator Operator { get; private set; }

        public IValueStatement Right { get; private set; }

        
        public ComparisonStatement(IValueStatement left, ComparisonOperator @operator, IValueStatement right)
        {
            Left     = left;
            Operator = @operator;
            Right    = right;
        }

        public void Visit(IStatementTranslator queryTranslator)
        {
            queryTranslator.TranslateComparisonStatement(this);
        }
    }
}
