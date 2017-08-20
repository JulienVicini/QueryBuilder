using EntityFramework.Extensions.Core.Queries.Statements.Operators;
using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Core.Queries.Statements.Builder
{
    public interface IStatementFactory<TEntity>
        where TEntity : class
    {
        IFilterStatement CreateFilterStatement( Expression<Func<TEntity, bool>> predicate );

        AssignStatement<TEntity> CreateAssignStatement(Expression<Action<TEntity>> assignementExpression);
    }
}
