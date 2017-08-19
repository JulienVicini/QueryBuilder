using EntityFramework.Extensions.ExpressionVisitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Tests.ExpressionVisitors
{
    [TestClass]
    public class SearchMethodCallExpressionVisitorTests
    {
        private IQueryable<string> _query;
        private SearchWhereMethodCallExpressionVisitor _visitor;

        [TestInitialize]
        public void Init()
        {
            // create queryable
            string[] strings = Enumerable.Range(0, 100).Select(index => "item number " + index)
                                                       .ToArray();

            _query = strings.AsQueryable();

            _visitor = new SearchWhereMethodCallExpressionVisitor();
        }

        [TestMethod]
        public void GetPredicateShouldReturnsNullWhenNotFiltering()
        {
            MethodCallExpression methodCallExpr = _visitor.GetMethodCall(_query);

            Assert.IsNull( methodCallExpr );
        }

        [TestMethod]
        public void GetPredicateThrowArgumentExceptionWhenMultipleFilter()
        {
            IQueryable<string> multipleFilterQuery = _query.Where(s => s != null)
                                                           .Where(s => s.Length > 10);

            // Multiple Where Call
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _visitor.GetMethodCall(multipleFilterQuery);
            });
        }

        [TestMethod]
        public void GetPredicateReturnsMethodCallExpression()
        {
            // build query
            Expression<Func<string, bool>> predicate = str => true;
            IQueryable<string> filteredQuery = _query.Where(predicate);

            // Act
            MethodCallExpression methodCallExpression = _visitor.GetMethodCall( filteredQuery );

            // Assert
            Assert.IsNotNull(methodCallExpression);

            UnaryExpression unaryExpr = methodCallExpression.Arguments[1] as UnaryExpression;
            Assert.IsNotNull(unaryExpr);
            Assert.AreEqual(unaryExpr.Operand, predicate);
        }
    }
}
