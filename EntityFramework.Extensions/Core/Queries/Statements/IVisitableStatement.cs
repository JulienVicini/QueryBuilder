namespace EntityFramework.Extensions.Core.Queries.Statements
{
    public interface IVisitableStatement
    {
        void Visit(IStatementTranslator queryTranslator);
    }
}
