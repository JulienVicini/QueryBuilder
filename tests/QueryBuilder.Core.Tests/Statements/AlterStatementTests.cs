using System;
using QueryBuilder.Core.Statements;
using Xunit;

namespace QueryBuilder.Core.Tests.Statements
{
    public class AlterStatementTests
    {
        [Fact]
        public void ConstructorShouldAssignValues()
        {
            // Arrange
            var tableName = "This is the tableName";
            var actionType = AlterTableStatement<object>.AlterType.Drop;

            // Act
            var statment = new AlterTableStatement<object>(
                tableName: tableName,
                type     : actionType
            );

            // Assert
            Assert.Same(tableName, statment.TableName);
            Assert.Equal(actionType, statment.Type);
        }

        [Fact]
        public void ConstructorShouldThrowArgumentExceptionWhenTableNameIsNull()
        {
            Assert.Throws<ArgumentException>(
                () => new AlterTableStatement<object>(null, AlterTableStatement<object>.AlterType.Create)
            );
        }

        [Fact]
        public void ConstructorShouldThrowArgumentExceptionWhenTableNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(
                () => new AlterTableStatement<object>(string.Empty, AlterTableStatement<object>.AlterType.Create)
            );
        }
    }
}
