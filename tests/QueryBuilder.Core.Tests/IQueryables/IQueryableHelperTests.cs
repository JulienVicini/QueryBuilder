using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Core.IQueryables;
using Xunit;

namespace QueryBuilder.Core.Tests.IQueryables
{
    public class IQueryableHelperTests
    {
        IQueryable<int> _queryable = new List<int>() { 1, 2, 3, 4 }.AsQueryable();

        [Fact]
        public void GetQueryPredicateThrowsArgumentNullExceptionWhenQuerayableIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => IQueryableHelper.GetQueryPredicate<int>(null)
            );
        }

        [Fact]
        public void GetQueryPredicateReturnsNullWhenNotFiltering()
        {
            
            IQueryable<int> query = _queryable;

            Assert.Null(IQueryableHelper.GetQueryPredicate(query));
        }

        [Fact]
        public void GetQueryPredicateReturnsPredicate()
        {

            // Arrange
            Expression<Func<int, bool>> predicate = p => p == 3;

            // Act
            IQueryable<int> query = _queryable.Where(predicate);

            // Assert
            Assert.Same(predicate, IQueryableHelper.GetQueryPredicate(query));
        }

        [Fact]
        public void GetQueryPredicateThrowsInvalidOperationExceptionWhenMultiplePredicateChained()
        {
            // Arrange and Act
            IQueryable<int> query = _queryable.Where(p => p == 3)
                                              .Where(p => p == 18);

            // Assert
            Assert.Throws<InvalidOperationException>(
                () => IQueryableHelper.GetQueryPredicate(query)
            );
        }
    }
}
