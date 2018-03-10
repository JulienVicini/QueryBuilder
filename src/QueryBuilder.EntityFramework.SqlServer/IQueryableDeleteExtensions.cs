using QueryBuilder.Core.IQueryables;
using QueryBuilder.Core.Statements;
using QueryBuilder.EntityFramework.SqlServer.Factories;
using System.Linq;

namespace QueryBuilder.EntityFramework.SqlServer
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
