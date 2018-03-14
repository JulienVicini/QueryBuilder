using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.Core.Statements;
using Xunit;

namespace QueryBuilder.Core.Tests.Statements
{
    public class DeleteStatementTests
    {
        [Fact]
        public void ConstructorShouldAssignPredicate()
        {
            // Arrange
            Expression<Func<Exception, bool>> predicate = ex => ex.Message == "test";

            // Act
            var deleteStatement = new DeleteStatement<Exception>(predicate);

            // Assert
            Assert.Same(predicate, deleteStatement.Predicate);
        }

        [Fact]
        public void ConstructorShouldAcceptNullPredicate()
        {
            // Arrage
            Expression<Func<Exception, bool>> predicate = null;

            // Act an Assert
            var statement = new DeleteStatement<Exception>(predicate);

            // Assert
            Assert.Null(statement.Predicate);
        }
    }
}
