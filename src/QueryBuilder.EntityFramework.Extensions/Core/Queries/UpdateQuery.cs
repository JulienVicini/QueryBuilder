using QueryBuilder.EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.Extensions.Core.Queries
{
    public class UpdateQuery<TEntity>
        where TEntity : class
    {
        public IEnumerable<MemberAssignment> Assignments { get; }

        public Expression<Func<TEntity, bool>> Predicate { get; }

        public UpdateQuery(IEnumerable<MemberAssignment> assignments, Expression<Func<TEntity, bool>> predicate)
        {
            ThrowHelper.ThrowIfNullOrEmpty(assignments, nameof(assignments));

            Assignments = assignments;
            Predicate   = predicate;
        }
    }
}
