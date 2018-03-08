using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Core.IQueryables;

namespace QueryBuilder.EntityFramework.Helpers
{
    public static class IQueryableHelpers
    {
        public static ObjectContext GetObjectContext<T>(IQueryable<T> queryable) where T : class
        {
            MethodCallExpression methodCallExpression = (queryable.Expression as MethodCallExpression)
                ?? throw new InvalidOperationException($"The parameter \"{nameof(queryable)}\" hasn't been create from EntityFramework's DbSet.");


            ConstantExpression constantExpression 
                = methodCallExpression?.Object as ConstantExpression
                    ?? (methodCallExpression.Arguments[0] as MethodCallExpression)?.Object as ConstantExpression; // TODO remove that crap

            ObjectQuery objectQuery = constantExpression?.Value as ObjectQuery;

            return objectQuery?.Context
                ?? throw new InvalidOperationException($"The parameter \"{nameof(queryable)}\" hasn't been created from a DbSet");
        }

        public static Expression<Func<T, bool>> GetQueryPredicate<T>(IQueryable<T> queryable) where T : class
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            MethodCallExpression methodCall = new SearchWhereMethodCallExpressionVisitor().GetMethodCall(queryable);

            if (methodCall == null)
                return null;
            else
                return GetPredicateFromMethodCall<T>(methodCall);
        }

        public static Expression<Func<T, bool>> GetPredicateFromMethodCall<T>(MethodCallExpression methodCallExpression)
        {
            UnaryExpression quote = methodCallExpression.Arguments[1] as UnaryExpression;

            if (quote == null || quote.NodeType != ExpressionType.Quote)
                throw new Exception(); // Use custom exception instead

            return (quote.Operand as Expression<Func<T, bool>>) ?? throw new Exception(); // Use custom exception instead
        }
    }
}
