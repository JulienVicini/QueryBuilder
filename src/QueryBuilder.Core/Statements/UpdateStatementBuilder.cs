using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.Core.Statements
{
    public class UpdateStatementBuilder<T>
    {
        public IQueryable<T> Queryable { get; }

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

        // TODO Throw Exception if valueExpression contains navigation property
        public UpdateStatementBuilder<T> SetValue<TValue>(Expression<Func<T, TValue>> memberExpression, Expression<Func<T, TValue>> valueExpression)
        {
            AppendMemberAssignment(memberExpression, valueExpression.Body);
            return this;
        }

        private void AppendMemberAssignment<TValue>(Expression<Func<T, TValue>> memberLambdaExpression, Expression value)
        {
            MemberExpression memberExpression = ExpressionHelper.GetMemberExpression(memberLambdaExpression);

            var memberAssignment = Expression.Bind(
                memberExpression.Member,
                value
            );

            AppendMemberAssignment(memberAssignment);
        }

        // TODO Throw Exception if memberAssignement contains navigation property
        public void AppendMemberAssignment(MemberAssignment memberAssignment)
        {
            Check.NotNull(memberAssignment, nameof(memberAssignment));

            if (memberAssignment.Member.DeclaringType != typeof(T))
                throw new ArgumentException(nameof(memberAssignment), $"The parameter must be a member of type \"{typeof(T).FullName}\" instead of \"{memberAssignment.Member.DeclaringType.FullName}\".");

            if (_assignements.Any(a => a.Member == memberAssignment.Member))
                throw new InvalidOperationException("duplicate member assignement");

            _assignements.Add(memberAssignment);
        }
    }
}
