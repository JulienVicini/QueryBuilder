using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Core.Statements;

namespace QueryBuilder.Core.Tests.Statements
{
    [TestClass]
    public class DeleteStatementTests
    {
        [TestMethod]
        public void ConstructorShouldAssignPredicate()
        {
            // Arrange
            Expression<Func<Exception, bool>> predicate = ex => ex.Message == "test";

            // Act
            var deleteStatement = new DeleteStatement<Exception>(predicate);

            // Assert
            Assert.AreSame(predicate, deleteStatement.Predicate);
        }

        [TestMethod]
        public void ConstructorShouldAcceptNullPredicate()
        {
            // Arrage
            Expression<Func<Exception, bool>> predicate = null;

            // Act an Assert
            var statement = new DeleteStatement<Exception>(predicate);

            // Assert
            Assert.IsNull(statement.Predicate);
        }
    }
}
