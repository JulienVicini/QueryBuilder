using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Core.Statements;

namespace QueryBuilder.Core.Tests.Statements
{
    [TestClass]
    public class AlterStatementTests
    {
        [TestMethod]
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
            Assert.AreSame(tableName, statment.TableName);
            Assert.AreEqual(actionType, statment.Type);
        }

        [TestMethod]
        public void ConstructorShouldThrowArgumentExceptionWhenTableNameIsNull()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new AlterTableStatement<object>(null, AlterTableStatement<object>.AlterType.Create)
            );
        }

        [TestMethod]
        public void ConstructorShouldThrowArgumentExceptionWhenTableNameIsEmpty()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new AlterTableStatement<object>(string.Empty, AlterTableStatement<object>.AlterType.Create)
            );
        }
    }
}
