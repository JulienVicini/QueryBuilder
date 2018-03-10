using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Core.Tests.Helpers
{
    [TestClass]
    public class CheckTests
    {
        #region NotNull

        [TestMethod]
        public void NotNullThrowsArgumentExceptionWhenValueIsNull()
        {
            string value = null;

            Assert.ThrowsException<ArgumentNullException>(() =>
                Check.NotNull(value, "parameter")
            );
        }

        [TestMethod]
        public void NotNullThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNull(data, paramterName)
            );
        }

        [TestMethod]
        public void NotNullThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            var paramterName = "   ";
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNull(data, paramterName)
            );
        }

        [TestMethod]
        public void NotNullShouldNotThrowException()
        {
            var paramterName = "parameter";
            var data = "NotEmpty";

            Check.NotNull(data, paramterName);
        }

        #endregion

        #region NotNullOrWhiteSpace

        [TestMethod]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenValueIsNull()
        {
            string value = null;

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrWhiteSpace(value, "parameter")
            );
        }

        [TestMethod]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenValueIsWhiteSpace()
        {
            var value = "   ";

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrWhiteSpace(value, "parameter")
            );
        }

        [TestMethod]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrWhiteSpace(data, paramterName)
            );
        }

        [TestMethod]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            var paramterName = "   ";
            var data = "NotEmpty";

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrEmpty(data, paramterName)
            );
        }

        [TestMethod]
        public void NotNullOrWhiteSpaceShouldNotThrowException()
        {
            var paramterName = "parameter";
            var data = "NotEmpty";

            Check.NotNullOrEmpty(data, paramterName);
        }

        #endregion

        #region NotNullOrEmpty

        [TestMethod]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenValueIsNull()
        {
            IEnumerable<int> nullData = null;

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrEmpty(nullData, "parameter")
            );
        }

        [TestMethod]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenValueIsEmpty()
        {
            var emptyData = Enumerable.Empty<int>();

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrEmpty(emptyData, "parameter")
            );
        }

        [TestMethod]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = new List<int>() { 1 };

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrEmpty(data, paramterName)
            );
        }

        [TestMethod]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            string paramterName = "   ";
            var data = new List<int>() { 1 };

            Assert.ThrowsException<ArgumentException>(() =>
                Check.NotNullOrEmpty(data, paramterName)
            );
        }

        [TestMethod]
        public void NotNullOrEmptyShouldNotThrowException()
        {
            string paramterName = "parameter";
            var data = new List<int>() { 1 };

            Check.NotNullOrEmpty(data, paramterName);
        }

        #endregion
    }
}
