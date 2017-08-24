using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Core.Tests.Helpers
{
    [TestClass]
    public class ThrowHelperTests
    {
        #region ThrowIfNull

        [TestMethod]
        public void ThrowIfNullThrowsArgumentExceptionWhenValueIsNull()
        {
            string value = null;

            Assert.ThrowsException<ArgumentNullException>(() =>
                ThrowHelper.ThrowIfNull(value, "parameter")
            );
        }

        [TestMethod]
        public void ThrowIfNullThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNull(data, paramterName)
            );
        }

        [TestMethod]
        public void ThrowIfNullThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            var paramterName = "   ";
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNull(data, paramterName)
            );
        }

        [TestMethod]
        public void ThrowIfNullShouldNotThrowException()
        {
            var paramterName = "parameter";
            var data = "NotEmpty";

            ThrowHelper.ThrowIfNull(data, paramterName);
        }

        #endregion

        #region ThrowIfNullOrWhiteSpace

        [TestMethod]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentExceptionWhenValueIsNull()
        {
            string value = null;

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrWhiteSpace(value, "parameter")
            );
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentExceptionWhenValueIsWhiteSpace()
        {
            var value = "   ";

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrWhiteSpace(value, "parameter")
            );
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrWhiteSpace(data, paramterName)
            );
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            var paramterName = "   ";
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrEmpty(data, paramterName)
            );
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpaceShouldNotThrowException()
        {
            var paramterName = "parameter";
            var data = "NotEmpty";

            ThrowHelper.ThrowIfNullOrEmpty(data, paramterName);
        }

        #endregion

        #region ThrowIfNullOrEmpty

        [TestMethod]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionWhenValueIsNull()
        {
            IEnumerable<int> nullData = null;

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrEmpty(nullData, "parameter")
            );
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionWhenValueIsEmpty()
        {
            var emptyData = Enumerable.Empty<int>();

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrEmpty(emptyData, "parameter")
            );
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = new List<int>() { 1 };

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrEmpty(data, paramterName)
            );
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            string paramterName = "   ";
            var data = new List<int>() { 1 };

            Assert.ThrowsException<ArgumentException>(() =>
                ThrowHelper.ThrowIfNullOrEmpty(data, paramterName)
            );
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyShouldNotThrowException()
        {
            string paramterName = "parameter";
            var data = new List<int>() { 1 };

            ThrowHelper.ThrowIfNullOrEmpty(data, paramterName);
        }

        #endregion
    }
}
