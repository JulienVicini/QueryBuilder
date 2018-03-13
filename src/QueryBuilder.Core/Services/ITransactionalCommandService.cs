using QueryBuilder.Core.Statements;

namespace QueryBuilder.Core.Services
{
    public interface ITransactionalCommandService<TEntity, TTransaction>
        where TEntity : class
    {
        int Alter(AlterTableStatement<TEntity> alterQuery, TTransaction transaction);
        int Delete(DeleteStatement<TEntity> deleteQuery, TTransaction transaction);
        int Merge(MergeStatement<TEntity> mergeQuery, TTransaction transaction);
        int Update(UpdateStatement<TEntity> updateQuery, TTransaction transaction);
    }
}
