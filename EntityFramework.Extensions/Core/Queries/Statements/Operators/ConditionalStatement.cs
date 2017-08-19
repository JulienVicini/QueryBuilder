namespace EntityFramework.Extensions.Core.Queries.Statements.Operators
{
    public class ConditionalStatement
        : IFilterStatement
    {
        public IFilterStatement Left { get; private set; }

        public ConditionalOperator Operator { get; private set; }

        public IFilterStatement Right { get; private set; }

        public ConditionalStatement(IFilterStatement left, ConditionalOperator @operator, IFilterStatement right)
        {
            Left     = left;
            Operator = @operator;
            Right    = right;
        }
    }
}
