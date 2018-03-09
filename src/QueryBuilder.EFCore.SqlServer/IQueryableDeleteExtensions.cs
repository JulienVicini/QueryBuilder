using System;
using System.Linq;
using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.IQueryables;
using QueryBuilder.EFCore.SqlServer.Factories;

namespace QueryBuilder.EFCore.SqlServer
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
