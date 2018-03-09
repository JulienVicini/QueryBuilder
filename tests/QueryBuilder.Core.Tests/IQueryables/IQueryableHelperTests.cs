using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Core.IQueryables;

namespace QueryBuilder.Core.Tests.IQueryables
{
    [TestClass]
    public class IQueryableHelperTests
    {
        IQueryable<int> _queryable = new List<int>() { 1, 2, 3, 4 }.AsQueryable();

        [TestMethod]
        public void GetQueryPredicateThrowsArgumentNullExceptionWhenQuerayableIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => IQueryableHelper.GetQueryPredicate<int>(null)
            );
        }

        [TestMethod]
        public void GetQueryPredicateReturnsNullWhenNotFiltering()
        {
            
            IQueryable<int> query = _queryable;

            Assert.IsNull(IQueryableHelper.GetQueryPredicate(query));
        }

        [TestMethod]
        public void GetQueryPredicateReturnsPredicate()
        {

            // Arrange
            Expression<Func<int, bool>> predicate = p => p == 3;

            // Act
            IQueryable<int> query = _queryable.Where(predicate);

            // Assert
            Assert.AreSame(predicate, IQueryableHelper.GetQueryPredicate(query));
        }

        [TestMethod]
        public void GetQueryPredicateThrowsInvalidOperationExceptionWhenMultiplePredicateChained()
        {
            // Arrange and Act
            IQueryable<int> query = _queryable.Where(p => p == 3)
                                              .Where(p => p == 18);

            // Assert
            Assert.ThrowsException<InvalidOperationException>(
                () => IQueryableHelper.GetQueryPredicate(query)
            );
        }
    }
}
