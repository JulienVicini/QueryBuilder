using QueryBuilder.Core.Services;

namespace QueryBuilder.SqlServer
{
    public interface ISQLServiceFactory<TEntity> where TEntity : class
    {
        ICommandService<TEntity> CreateCommandService();

        IBulkInsertService<TEntity> CreateBulkInsert();

        IBulkMergeService<TEntity> CreateBulkMerge();
    }
}
