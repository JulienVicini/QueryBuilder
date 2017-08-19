namespace EntityFramework.Extensions.Core.Queries.Statements.Values
{
    public class ConstantStatement<T>
        : IValueStatement
    {
        public T Value { get; private set; }

        public ConstantStatement(T value)
        {
            Value = value;
        }

        public void Visit(IStatementTranslator queryTranslator)
        {
            queryTranslator.TranslateConstantStatement(this);
        }
    }
}
