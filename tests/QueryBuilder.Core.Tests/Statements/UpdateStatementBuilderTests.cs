using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.Tests.FakeModels;
using Xunit;

namespace QueryBuilder.Core.Tests.Statements
{
    public class UpdateStatementBuilderTests
    {
        #region  Constructor

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenQueryableIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new UpdateStatementBuilder<TestClass>(null)
            );
        }

        [Fact]
        public void ConstructorShouldHoldQueryableReference()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            // Act
            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            // Assert
            Assert.Same(queryable, statementBuilder.Queryable);
        }

        #endregion

        #region AppendMemberAssignment

        [Fact]
        public void AppendMemberAssignmentThrowsArgumentNullExceptionWhenExpressionIsNull()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            // Act and Assert
            Assert.Throws<ArgumentNullException>(
                () => statementBuilder.AppendMemberAssignment(null)
            );
        }

        [Fact]
        public void AppendMemberAssignmentThrowsArgumentExceptionWhenMemberIsNotOfTheRightType()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            MemberAssignment assignment
                = Expression.Bind(
                        typeof(OtherClass).GetProperty(nameof(OtherClass.Id)),
                        Expression.Constant(3)
                    );

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => statementBuilder.AppendMemberAssignment(assignment)
            );
        }

        [Fact]
        public void AppendMemberAssignmentThrowsInvalidOperationExceptionOnDuplicateMemberAssignement()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            MemberAssignment assignment
                = Expression.Bind(
                        typeof(TestClass).GetProperty(nameof(TestClass.Id)),
                        Expression.Constant(3)
                    );

            // Act and Assert
            statementBuilder.AppendMemberAssignment(assignment);
            Assert.Throws<InvalidOperationException>(
                () => statementBuilder.AppendMemberAssignment(assignment)
            );
        }

        [Fact]
        public void AppendMemberAssignmentShouldAppendAssignment()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            MemberAssignment assignment
                = Expression.Bind(
                        typeof(TestClass).GetProperty(nameof(TestClass.Id)),
                        Expression.Constant(3)
                    );

            // Act
            statementBuilder.AppendMemberAssignment(assignment);

            // Assert
            Assert.Single(statementBuilder.Assignements);
            Assert.Same(assignment, statementBuilder.Assignements.First());
        }

        #endregion

        #region SetValue

        [Fact]
        public void SetValueShouldAppendAssignment()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            // Act
            statementBuilder.SetValue(s => s.Id, 3);

            // Assert
            Assert.Single(statementBuilder.Assignements);

            MemberAssignment assignment = statementBuilder.Assignements.Single();
            Assert.Same(typeof(TestClass).GetProperty(nameof(TestClass.Id)), assignment.Member    );
            Assert.IsAssignableFrom<ConstantExpression>(assignment.Expression);

            var constantExpression = assignment.Expression as ConstantExpression;
            Assert.IsType<int>(constantExpression.Value);
            Assert.Equal(3, constantExpression.Value);
        }

        [Fact]
        public void SetValueShouldAppendIncreaseAssignment()
        {
            // Arrange
            IQueryable<TestClass> queryable = new List<TestClass>().AsQueryable();

            var statementBuilder = new UpdateStatementBuilder<TestClass>(queryable);

            PropertyInfo propertyToAssign = typeof(TestClass).GetProperty(nameof(TestClass.Id));

            // Act
            statementBuilder.SetValue(s => s.Id, s => s.Id + 3);

            // Assert
            Assert.Single(statementBuilder.Assignements);

            MemberAssignment assignment = statementBuilder.Assignements.Single();
            Assert.Same(propertyToAssign, assignment.Member);
            Assert.IsAssignableFrom<BinaryExpression>(assignment.Expression);

            var binaryExpression = assignment.Expression as BinaryExpression;
            Assert.IsAssignableFrom<MemberExpression>(binaryExpression.Left);
            Assert.IsAssignableFrom<ConstantExpression>(binaryExpression.Right);

            var memberExpression = binaryExpression.Left as MemberExpression;
            Assert.Same(propertyToAssign, memberExpression.Member);

            Assert.Equal(ExpressionType.Add, binaryExpression.NodeType);

            var constantExpression = binaryExpression.Right as ConstantExpression;
            Assert.IsType<int>(constantExpression.Value);
            Assert.Equal(3, constantExpression.Value);
        }

        #endregion
    }
}
