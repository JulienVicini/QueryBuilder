using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Tests
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
