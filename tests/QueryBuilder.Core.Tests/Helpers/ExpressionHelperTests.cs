using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace QueryBuilder.Core.Tests.Helpers
{
    public class ExpressionHelperTests
    {
        private class FakeModel
        {
            public int PropertyToTest { get; set; }
        }

        #region GetMemberExpression

        [Fact]
        public void GetMemberExpressionThrowsArgumentNullExceptionWhenExpressionIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => ExpressionHelper.GetMemberExpression<CheckTests, bool>(null)
            );
        }

        [Fact]
        public void GetMemberExpressionThrowsArgumentExceptionWhenExpressionIsNotMemberExpression()
        {
            Assert.Throws<ArgumentException>(
                () => ExpressionHelper.GetMemberExpression<CheckTests, int>(_ => 3)
            );
        }

        [Fact]
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

            Assert.Equal(propertyToTest, memberExpression.Member.Name);
            Assert.Equal(typeof(FakeModel), memberExpression.Member.DeclaringType);
            Assert.Equal(member, memberExpression.Member);
        }

        #endregion

        #region MakeMemberExpression

        [Fact]
        public void MakeMemberExpressionThrowsArgumentNullExceptionWhenMemberAssignementIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => {
                ExpressionHelper.MakeMemberExpression(null);
            });
        }

        [Fact]
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

            Assert.Equal(memberInfo, expression.Member);
        }

        #endregion

        #region MakeAssign

        [Fact]
        public void MakeAssignThrowsArgumentNullExceptionWhenMemberAssignementIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => {
                ExpressionHelper.MakeAssign(null);
            });
        }

        [Fact]
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
            Assert.IsAssignableFrom<MemberExpression>(expression.Left);
            Assert.Equal(memberInfo, ((MemberExpression)expression.Left).Member);

            // Right
            Assert.IsAssignableFrom<ConstantExpression>(expression.Right);
            Assert.Equal(3, ((ConstantExpression)expression.Right).Value);
        }

        #endregion

        #region GetSelectedMemberInAnonymousType

        [Fact]
        public void GetSelectedMemberInAnonymousTypeThrowsArgumentNullExceptionWhenExpressionIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => ExpressionHelper.GetSelectedMemberInAnonymousType<FakeModel, FakeModel>(null)
            );
        }

        [Fact]
        public void GetSelectedMemberInAnonymousTypeThrowsInvalidOperationExceptionWhenExpressionBodyIsNotNewExpression()
        {
            Assert.Throws<InvalidOperationException>(
                () => ExpressionHelper.GetSelectedMemberInAnonymousType((FakeModel model) => model)
            );
        }

        [Fact]
        public void GetSelectedMemberInAnonymousTypeThrowsArgumentExceptionWhenExpressionArgumentsIsNotAMemberExpression()
        {
            Assert.Throws<ArgumentException>(() => {
                ExpressionHelper.GetSelectedMemberInAnonymousType((FakeModel model) => new { data = 3 });
            });
        }

        [Fact]
        public void GetSelectedMemberShouldReturnsSelectedMember()
        {
            // Arrange
            Type expectedParameterType = typeof(FakeModel);
            MemberInfo expectedMember = typeof(FakeModel).GetProperty(nameof(FakeModel.PropertyToTest));

            // Act
            IEnumerable<MemberExpression> members 
                = ExpressionHelper.GetSelectedMemberInAnonymousType((FakeModel model) => new { model.PropertyToTest });

            // Assert Result COunt
            Assert.Single(members);
            MemberExpression selectedMember = members.First();

            // Asset Expression parameter
            Assert.IsAssignableFrom<ParameterExpression>(selectedMember.Expression);
            Assert.Equal(typeof(FakeModel), selectedMember.Expression.Type);

            // Assert MemberINfo
            Assert.Equal(expectedMember, selectedMember.Member);
        }

        #endregion
    }
}
