using System;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.Core.IQueryables
{
    public static class IQueryableHelper
    {
        public static Expression<Func<T, bool>> GetQueryPredicate<T>(IQueryable<T> queryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            MethodCallExpression methodCall = new SearchWhereMethodCallExpressionVisitor().GetMethodCall(queryable);

            if (methodCall == null)
                return null;
            else
                return GetPredicateFromMethodCall<T>(methodCall);
        }

        // TODO unit test
        public static Expression<Func<T, bool>> GetPredicateFromMethodCall<T>(MethodCallExpression methodCallExpression)
        {
            UnaryExpression quote = methodCallExpression.Arguments[1] as UnaryExpression;

            if (quote == null || quote.NodeType != ExpressionType.Quote)
                throw new Exception(); // Use custom exception instead

            return (quote.Operand as Expression<Func<T, bool>>) ?? throw new Exception(); // Use custom exception instead
        }
    }
}
