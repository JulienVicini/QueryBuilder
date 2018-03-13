using QueryBuilder.Core.Statements;

namespace QueryBuilder.Core.Services
{
    public interface ICommandService<TEntity> where TEntity : class
    {
        int Alter(AlterTableStatement<TEntity> alterQuery);
        int Delete(DeleteStatement<TEntity> deleteQuery);
        int Merge(MergeStatement<TEntity> mergeQuery);
        int Update(UpdateStatement<TEntity> updateQuery);
    }
}