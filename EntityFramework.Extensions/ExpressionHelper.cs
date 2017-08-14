using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions
{
    public static class ExpressionHelper
    {
        public static string GetMemberName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;

            return memberExpression.Member.Name;
        }
    }
}
