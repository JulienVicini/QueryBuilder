using QueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.Core.Statements
{
    public class UpdateStatementBuilder<T>
    {
        public IQueryable<T> Queryable { get; private set; }

        public IEnumerable<MemberAssignment> Assignements => _assignements.AsReadOnly();
        private readonly List<MemberAssignment> _assignements;

        public UpdateStatementBuilder(IQueryable<T> queryable)
        {
            _assignements = new List<MemberAssignment>();
            Queryable     = queryable ?? throw new ArgumentNullException(nameof(queryable));
        }

        public UpdateStatementBuilder<T> SetValue<TValue>(Expression<Func<T, TValue>> memberExpression, TValue value)
        {
            var constantExpression = Expression.Constant(value);

            AppendMemberAssignment(memberExpression, constantExpression);

            return this;
        }

        public UpdateStatementBuilder<T> SetValue<TValue>(Expression<Func<T, TValue>> memberExpression, Expression<Func<T, TValue>> valueExpression)
        {
            AppendMemberAssignment(memberExpression, valueExpression.Body);
            return this;
        }

        // TODO refactor this
        public void AppendMemberAssignment<TValue>(Expression<Func<T, TValue>> memberLambdaExpression, Expression value)
        {
            MemberExpression memberExpression = ExpressionHelper.GetMemberExpression(memberLambdaExpression);

            if (_assignements.Any(a => a.Member.Name == memberExpression.Member.Name))
                throw new Exception("duplicate member assignement");

            var memberAssignment = Expression.Bind(
                memberExpression.Member,
                value
            );

            _assignements.Add(memberAssignment);
        }
    }
}
