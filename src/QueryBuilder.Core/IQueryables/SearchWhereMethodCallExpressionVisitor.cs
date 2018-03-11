using System;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.Core.IQueryables
{
    public class SearchWhereMethodCallExpressionVisitor
        : ExpressionVisitor
    {
        #region Members

        private MethodCallExpression _expressionCall;
        private bool _hasBeenCalled;

        #endregion

        #region Public Methods

        public MethodCallExpression GetMethodCall<T>(IQueryable<T> queryable)
        {
            if (_hasBeenCalled)
                throw new InvalidOperationException($"The method \"{nameof(SearchWhereMethodCallExpressionVisitor)}.{nameof(GetMethodCall)}\" can be called only once.");

            _hasBeenCalled = true;
            _expressionCall = null;

            Visit(queryable.Expression);

            return _expressionCall;
        }

        #endregion

        #region Visit Methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == node.Method.Name)
            {
                if (_expressionCall != null)
                    throw new InvalidOperationException($"Multiple call of \"{nameof(Queryable)}.{nameof(Queryable.Where)}\" where found.");
                _expressionCall = node;
            }

            if (node.Arguments.Count > 0)
                Visit(node.Arguments[0]);

            return node;
        }

        #endregion
    }
}
