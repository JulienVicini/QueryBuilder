using System;
using System.Linq.Expressions;
using QueryBuilder.Core.Helpers;

namespace QueryBuilder.Core.Statements
{
    public class DeleteStatement<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; }

        public DeleteStatement(Expression<Func<TEntity, bool>> predicate)
        {
            ThrowHelper.ThrowIfNull(predicate, nameof(predicate));

            Predicate = predicate;
        }
    }
}
