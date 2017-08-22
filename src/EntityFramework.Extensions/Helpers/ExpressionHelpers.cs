using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Helpers
{
    public static class ExpressionHelpers
    {
        public static MemberExpression GetMemberExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            MemberExpression memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException($"The parameter \"{nameof(expression)}\" must a be a lambda expression of a member.");

            return memberExpression;
        }

        public static MemberExpression MakeMemberExpression(MemberAssignment memberAssignement)
        {
            return Expression.MakeMemberAccess(
                Expression.Parameter(memberAssignement.Member.DeclaringType),
                memberAssignement.Member
            );
        }

        public static BinaryExpression MakeAssign(MemberAssignment memberAssignment)
        {
            return Expression.Assign(
                MakeMemberExpression(memberAssignment),
                memberAssignment.Expression
            );
        }

        public static IEnumerable<MemberExpression> GetSelectedMemberInAnonymousType<TEntity, TColumns>(Expression<Func<TEntity, TColumns>> expression)
        {
            var newExpression = expression.Body as NewExpression;

            return newExpression.Arguments.Select(arg => (MemberExpression)arg)
                                          .ToList();
             //expression.Body;
        }
    }
}
