using QueryBuilder.Core.IQueryables;
using QueryBuilder.Core.Statements;
using QueryBuilder.EF6.SqlServer.Factories;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.EF6.SqlServer
{
    public static class IQueryableUpdateExtensions
    {
        public static UpdateStatementBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, Expression<Func<T, TValue>> valueExpression)
        {
            return new UpdateStatementBuilder<T>(queryable)
                        .SetValue(memberExpression, valueExpression);
        }

        public static UpdateStatementBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, TValue value)
        {
            return new UpdateStatementBuilder<T>(queryable)
                        .SetValue(memberExpression, value);
        }

        public static int Update<T>(this UpdateStatementBuilder<T> updateBuilder) where T : class
        {
            IQueryable<T> queryable = updateBuilder.Queryable;

            // Create Query
            var query = new UpdateStatement<T>(
                updateBuilder.Assignements,
                IQueryableHelper.GetQueryPredicate(queryable)
            );

            // Create Statement Facade
            StatementFacade<T> statementFacade
                = new StatementFacadeFactory<T>().CreateFacade(queryable);

            return statementFacade.Update(query);
        }
    }
}
