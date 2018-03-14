using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.Tests.FakeModels;
using Xunit;

namespace QueryBuilder.Core.Tests.Statements
{
    public class UpdateStatementTests
    {
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenAssignementsIsNull()
        {
            // Arrange
            IEnumerable<MemberAssignment> assignments   = null;
            Expression<Func<TestClass, bool>> predicate = _ => true;

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => new UpdateStatement<TestClass>(assignments, predicate)
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenAssignementsIsEmpty()
        {
            // Arrange
            IEnumerable<MemberAssignment> assignments   = Enumerable.Empty<MemberAssignment>();
            Expression<Func<TestClass, bool>> predicate = _ => true;

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => new UpdateStatement<TestClass>(assignments, predicate)
            );
        }

        [Fact]
        public void ConstructorShouldAssignValues()
        {
            // Arrange
            MemberAssignment assignement
                = Expression.Bind(
                        typeof(TestClass).GetProperty(nameof(TestClass.Id)),
                        Expression.Constant(3)
                    );            
            IEnumerable<MemberAssignment> assignments 
                = new List<MemberAssignment>() { assignement };

            Expression<Func<TestClass, bool>> predicate = _ => true;

            // Act
            var updateStatement = new UpdateStatement<TestClass>(assignments, predicate);

            // Assert
            Assert.Same(assignments, updateStatement.Assignments);
            Assert.Same(predicate  , updateStatement.Predicate  );
        }
    }
}
