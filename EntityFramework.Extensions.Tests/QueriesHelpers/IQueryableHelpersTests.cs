using EntityFramework.Extensions.Helpers;
using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Tests.QueriesHelpers
{
    [TestClass]
    public class IQueryableHelpersTests
    {
        #region GetObjectContext

        [TestMethod]
        public void GetObjectContextReturnsObjectContext()
        {
            using(var dbContext = new TestDbContext())
            {
                // From IQueryable
                ObjectContext queryableContext = IQueryableHelpers.GetObjectContext(
                    dbContext.Parents
                );

                // From DbContext
                var objectContextAdapter = (IObjectContextAdapter)dbContext;
                ObjectContext dbContextObjectContext = objectContextAdapter.ObjectContext;

                Assert.AreEqual(dbContextObjectContext, queryableContext);
            }
        }

        [TestMethod]
        public void GetObjectContextThrowsExceptionWhenEmptyEnumerable()
        {
            IQueryable<object> queryable = Enumerable.Empty<object>().AsQueryable();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                IQueryableHelpers.GetObjectContext(queryable);
            });
        }

        [TestMethod]
        public void GetObjectContextThrowsExceptionWhenList()
        {
            IQueryable<object> queryable = new List<object>() { 1, 2, 3 }.AsQueryable();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                IQueryableHelpers.GetObjectContext(queryable);
            });
        }

        #endregion

        #region GetQueryPredicate

        [TestMethod]
        public void GetQueryPredicateThrowsArgumentNullExceptionWhenQuerayableIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => IQueryableHelpers.GetQueryPredicate<Parent>(null));
        }

        [TestMethod]
        public void GetQueryPredicateReturnsNullWhenNotFiltering()
        {
            using(var dbContext = new TestDbContext())
            {
                IQueryable<Parent> query = dbContext.Parents;

                Assert.AreEqual( null, IQueryableHelpers.GetQueryPredicate(query) );
            }
        }

        [TestMethod]
        public void GetQueryPredicateReturnsPredicate()
        {
            using(var dbContext = new TestDbContext())
            {
                // prepare query
                Expression<Func<Parent, bool>> predicate = p => p.Id == 3;

                IQueryable<Parent> query = dbContext.Parents.Where(predicate);
                
                // act
                Assert.AreEqual( predicate, IQueryableHelpers.GetQueryPredicate(query) );
            }
        }

        [TestMethod]
        public void GetQueryPredicateThrowsInvalidOperationExceptionWhenMultiplePredicateChained()
        {
            using (var dbContext = new TestDbContext())
            {
                // prepare query
                IQueryable<Parent> query = dbContext.Parents.Where(p => p.Id == 3)
                                                            .Where(p => p.SecondVariable == 18);

                // act
                Assert.ThrowsException<InvalidOperationException>( () => IQueryableHelpers.GetQueryPredicate(query) );
            }
        }

        #endregion
    }
}
