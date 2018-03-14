using System;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Core.IQueryables;
using Xunit;

namespace QueryBuilder.Core.Tests.IQueryables
{
    public class SearchMethodCallExpressionVisitorTests
    {
        private IQueryable<string> _query;
        private SearchWhereMethodCallExpressionVisitor _visitor;

        public SearchMethodCallExpressionVisitorTests()
        {
            // create queryable
            string[] strings = Enumerable.Range(0, 100).Select(index => "item number " + index)
                                                       .ToArray();

            _query = strings.AsQueryable();

            _visitor = new SearchWhereMethodCallExpressionVisitor();
        }

        [Fact]
        public void GetPredicateShouldReturnsNullWhenNotFiltering()
        {
            MethodCallExpression methodCallExpr = _visitor.GetMethodCall(_query);

            Assert.Null(methodCallExpr);
        }

        [Fact]
        public void GetMethodCallThrowsInvalideOperatorExceptionWhenCalledTwice()
        {
            // Arrange
            Expression<Func<string, bool>> predicate = str => true;
            IQueryable<string> filteredQuery = _query.Where(predicate);

            // Act
            _visitor.GetMethodCall(filteredQuery);

            // Assert
            Assert.Throws<InvalidOperationException>(
                () => _visitor.GetMethodCall(filteredQuery)
            );
        }

        [Fact]
        public void GetPredicateThrowArgumentExceptionWhenMultipleFilter()
        {
            IQueryable<string> multipleFilterQuery = _query.Where(s => s != null)
                                                           .Where(s => s.Length > 10);

            // Multiple Where Call
            Assert.Throws<InvalidOperationException>(() =>
            {
                _visitor.GetMethodCall(multipleFilterQuery);
            });
        }

        [Fact]
        public void GetPredicateReturnsMethodCallExpression()
        {
            // build query
            Expression<Func<string, bool>> predicate = str => true;
            IQueryable<string> filteredQuery = _query.Where(predicate);

            // Act
            MethodCallExpression methodCallExpression = _visitor.GetMethodCall(filteredQuery);

            // Assert
            Assert.NotNull(methodCallExpression);

            UnaryExpression unaryExpr = methodCallExpression.Arguments[1] as UnaryExpression;
            Assert.NotNull(unaryExpr);
            Assert.Equal(unaryExpr.Operand, predicate);
        }
    }
}
