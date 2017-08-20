using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Core.Queries
{
    public class UpdateQuery<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }


        public UpdateQuery(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }
    }
}
