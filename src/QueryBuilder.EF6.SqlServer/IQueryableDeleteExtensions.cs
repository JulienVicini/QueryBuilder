using QueryBuilder.Core.IQueryables;
using QueryBuilder.Core.Statements;
using QueryBuilder.EF6.SqlServer.Factories;
using System.Linq;

namespace QueryBuilder.EF6.SqlServer
{
    public static class IQueryableDeleteExtensions
    {
        public static int Delete<T>(this IQueryable<T> queryable)
            where T : class
        {
            // BuilderQuery
            var query = new DeleteStatement<T>(IQueryableHelper.GetQueryPredicate(queryable));

            // Create Statement Facade
            StatementFacade<T> statementFace = new StatementFacadeFactory<T>()
                                                    .CreateFacade(queryable);

            return statementFace.Delete(query);
        }
    }
}
