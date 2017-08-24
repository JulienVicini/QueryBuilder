using System;
using System.Linq.Expressions;

namespace QueryBuilder.Core.Statements
{
    public class DeleteStatement<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }

        public DeleteStatement(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }
    }
}
