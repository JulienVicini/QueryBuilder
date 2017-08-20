using EntityFramework.Extensions.Core.Queries.Statements.Operators;
using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Core.Queries.Statements.Builder
{
    public class StatementFactory<TEntity>
        : IStatementFactory<TEntity>
        where TEntity : class
    {
        public AssignStatement<TEntity> CreateAssignStatement(Expression<Action<TEntity>> assignementExpression)
        {
            throw new NotImplementedException();
        }

        #region Filter Statement

        public IFilterStatement CreateFilterStatement(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
