using System;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.Extensions.Core.Queries
{
    public class DeleteQuery<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }

        public DeleteQuery(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }
    }
}
