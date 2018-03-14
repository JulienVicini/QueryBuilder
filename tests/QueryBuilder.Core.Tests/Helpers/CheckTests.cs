using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QueryBuilder.Core.Tests.Helpers
{
    public class CheckTests
    {
        #region NotNull

        [Fact]
        public void NotNullThrowsArgumentExceptionWhenValueIsNull()
        {
            string value = null;

            Assert.Throws<ArgumentNullException>(() =>
                Check.NotNull(value, "parameter")
            );
        }

        [Fact]
        public void NotNullThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = "NotEmpty";

            Assert.Throws<ArgumentException>(() =>
                Check.NotNull(data, paramterName)
            );
        }

        [Fact]
        public void NotNullThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            var paramterName = "   ";
            var data = "NotEmpty";

            Assert.Throws<ArgumentException>(() =>
                Check.NotNull(data, paramterName)
            );
        }

        [Fact]
        public void NotNullShouldNotThrowException()
        {
            var paramterName = "parameter";
            var data = "NotEmpty";

            Check.NotNull(data, paramterName);
        }

        #endregion

        #region NotNullOrWhiteSpace

        [Fact]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenValueIsNull()
        {
            string value = null;

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrWhiteSpace(value, "parameter")
            );
        }

        [Fact]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenValueIsWhiteSpace()
        {
            var value = "   ";

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrWhiteSpace(value, "parameter")
            );
        }

        [Fact]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = "NotEmpty";

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrWhiteSpace(data, paramterName)
            );
        }

        [Fact]
        public void NotNullOrWhiteSpaceThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            var paramterName = "   ";
            var data = "NotEmpty";

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrEmpty(data, paramterName)
            );
        }

        [Fact]
        public void NotNullOrWhiteSpaceShouldNotThrowException()
        {
            var paramterName = "parameter";
            var data = "NotEmpty";

            Check.NotNullOrEmpty(data, paramterName);
        }

        #endregion

        #region NotNullOrEmpty

        [Fact]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenValueIsNull()
        {
            IEnumerable<int> nullData = null;

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrEmpty(nullData, "parameter")
            );
        }

        [Fact]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenValueIsEmpty()
        {
            var emptyData = Enumerable.Empty<int>();

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrEmpty(emptyData, "parameter")
            );
        }

        [Fact]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenParameterNameIsNull()
        {
            string paramterName = null;
            var data = new List<int>() { 1 };

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrEmpty(data, paramterName)
            );
        }

        [Fact]
        public void NotNullOrEmptyThrowsArgumentExceptionWhenParameterNameIsWhiteSpace()
        {
            string paramterName = "   ";
            var data = new List<int>() { 1 };

            Assert.Throws<ArgumentException>(() =>
                Check.NotNullOrEmpty(data, paramterName)
            );
        }

        [Fact]
        public void NotNullOrEmptyShouldNotThrowException()
        {
            string paramterName = "parameter";
            var data = new List<int>() { 1 };

            Check.NotNullOrEmpty(data, paramterName);
        }

        #endregion
    }
}
