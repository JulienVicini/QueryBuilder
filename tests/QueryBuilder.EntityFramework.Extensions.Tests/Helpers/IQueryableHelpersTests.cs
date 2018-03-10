using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.EntityFramework.Extensions.Tests.Context;
using QueryBuilder.EntityFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace QueryBuilder.EntityFramework.Extensions.Tests.QueriesHelpers
{
    [TestClass]
    public class IQueryableHelpersTests
    {
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
    }
}
