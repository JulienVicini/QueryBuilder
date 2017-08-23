using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryBuilder.Core.Tests.Helpers
{
    [TestClass]
    public class ExpressionHelperTests
    {
        private class FakeModel
        {
            public int PropertyToTest { get; set; }
        }

        #region GetMemberExpression

        [TestMethod]
        public void GetMemberExpressionThrowsArgumentNullExceptionWhenExpressionIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                ExpressionHelper.GetMemberExpression<ThrowHelperTests, bool>(null)
            );
        }

        [TestMethod]
        public void GetMemberExpressionShouldReturnMemberExpression()
        {
            // Act
            Expression<Func<FakeModel, int>> expr
                = m => m.PropertyToTest;

            MemberExpression memberExpression = ExpressionHelper.GetMemberExpression(expr);

            // Assert
            const string propertyToTest = nameof(FakeModel.PropertyToTest);
            MemberInfo member = typeof(FakeModel).GetMember(propertyToTest)
                                                 .Single();

            Assert.AreEqual(propertyToTest, memberExpression.Member.Name);
            Assert.AreEqual(typeof(FakeModel), memberExpression.Member.DeclaringType);
            Assert.AreEqual(member, memberExpression.Member);
        }

        #endregion

        #region MakeMemberExpression

        [TestMethod]
        public void MakeMemberExpressionThrowsArgumentNullExceptionWhenMemberAssignementIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => {
                ExpressionHelper.MakeMemberExpression(null);
            });
        }

        [TestMethod]
        public void MakeMemberExpressionShouldReturnsMemberExpression()
        {
            // Create Test
            MemberInfo memberInfo = typeof(FakeModel).GetMember(nameof(FakeModel.PropertyToTest))
                                                     .Single();

            MemberAssignment assignment
                = Expression.Bind(
                    memberInfo,
                    Expression.Constant(3)
                );

            // Act
            MemberExpression expression = ExpressionHelper.MakeMemberExpression(assignment);

            Assert.AreEqual(memberInfo, expression.Member);
        }

        #endregion

        #region MakeAssign

        [TestMethod]
        public void MakeAssignThrowsArgumentNullExceptionWhenMemberAssignementIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => {
                ExpressionHelper.MakeAssign(null);
            });
        }

        [TestMethod]
        public void MakeAssignShouldReturnsBinaryExpression()
        {
            // Create Test
            MemberInfo memberInfo = typeof(FakeModel).GetMember(nameof(FakeModel.PropertyToTest))
                                                     .Single();

            MemberAssignment assignment
                = Expression.Bind(
                    memberInfo,
                    Expression.Constant(3)
                );

            // Act
            BinaryExpression expression = ExpressionHelper.MakeAssign(assignment);

            // Left
            Assert.IsInstanceOfType(expression.Left, typeof(MemberExpression));
            Assert.AreEqual(memberInfo, ((MemberExpression)expression.Left).Member);

            // Right
            Assert.IsInstanceOfType(expression.Right, typeof(ConstantExpression));
            Assert.AreEqual(3, ((ConstantExpression)expression.Right).Value);
        }

        #endregion
    }
}
