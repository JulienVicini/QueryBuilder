namespace EntityFramework.Extensions.Core.Queries.Statements.Operators
{
    public class ArithmeticStatement
        : IValueStatement
    {
        public IValueStatement Left { get; private set; }

        public ArithmeticOperator Operator { get; private set; }

        public IValueStatement Right { get; private set; }


        public ArithmeticStatement(IValueStatement left, ArithmeticOperator @operator, IValueStatement right)
        {
            Left     = left;
            Operator = @operator;
            Right    = right;
        }
    }
}
