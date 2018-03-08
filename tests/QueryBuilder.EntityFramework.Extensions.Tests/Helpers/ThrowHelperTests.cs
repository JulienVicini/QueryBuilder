using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.EntityFramework.Extensions.Tests.Helpers
{
    [TestClass]
    public class ThrowHelperTests
    {
        [TestMethod]
        public void ThrowIfNullOrEmptyThrowsArgumentWhenArgumentNameIsNullOrWhiteSpace()
        {
            var objectList = new List<object>() { new object() };

            Assert.ThrowsException<ArgumentException>(() => ThrowHelper.ThrowIfNullOrEmpty(objectList, null));
            Assert.ThrowsException<ArgumentException>(() => ThrowHelper.ThrowIfNullOrEmpty(objectList, string.Empty));
            Assert.ThrowsException<ArgumentException>(() => ThrowHelper.ThrowIfNullOrEmpty(objectList, " "));
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyThrowsArgumentWhenEnumerableIsNull()
        {
            const string argumentName = "argumentName";

            Assert.ThrowsException<ArgumentException>(() => ThrowHelper.ThrowIfNullOrEmpty(null as IEnumerable<int>, argumentName));
            Assert.ThrowsException<ArgumentException>(() => ThrowHelper.ThrowIfNullOrEmpty(Enumerable.Empty<int>() , argumentName));
        }
    }
}
