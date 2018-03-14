using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.Tests.FakeModels;
using Xunit;

namespace QueryBuilder.Core.Tests.Statements
{
    public class MergeStatementTests
    {
        #region Init

        private readonly List<MemberExpression> _mergeKeys;

        public MergeStatementTests()
        {
            Expression<Func<TestClass, int>> memberExpression = t => t.Id;

            _mergeKeys = new List<MemberExpression>()
            {
                (MemberExpression)memberExpression.Body
            };
        }

        #endregion

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenKeysIsNull()
        {
            Assert.Throws<ArgumentException>(
                () => new MergeStatement<TestClass>(null, "temporaryTableName", MergeStatement<TestClass>.MergeType.InsertOnly)
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenKeysIsEmpty()
        {
            Assert.Throws<ArgumentException>(
                () => new MergeStatement<TestClass>(Enumerable.Empty<MemberExpression>(), "temporaryTableName", MergeStatement<TestClass>.MergeType.InsertOnly)
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ConstructorThrowsArgumentExceptionWhenTableNameIsNullOrWhitespace(string tableName)
        {
            Assert.Throws<ArgumentException>(
                () => new MergeStatement<TestClass>(_mergeKeys, tableName, MergeStatement<TestClass>.MergeType.InsertOnly)
            );
        }

        [Fact]
        public void ConstructorShouldAssignValues()
        {
            // Arrange
            const string tableName = "TemporaryTableName";
            var mergeType = MergeStatement<TestClass>.MergeType.InsertOrUpdate;

            // Act
            var mergeStatement = new MergeStatement<TestClass>(
                _mergeKeys, tableName, mergeType
            );

            // Assert
            Assert.Same(_mergeKeys, mergeStatement.Keys              );
            Assert.Same(tableName , mergeStatement.TemporaryTableName);
            Assert.Equal(mergeType, mergeStatement.Type              );
        }
    }
}
