using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.EntityFramework.Extensions.Tests.Context;
using System;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.Extensions.Tests
{
    [TestClass]
    public class RandDTest
    {
        [TestMethod]
        public void Dummy()
        {
            Expression<Func<Parent, int>> parent1 = p => p.Id,
                                          parent2 = p => p.Id;

            bool areEquals = parent1 == parent2;
        }
    }
}
