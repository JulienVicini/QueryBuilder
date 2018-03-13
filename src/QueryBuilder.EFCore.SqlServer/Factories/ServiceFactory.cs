using System.Linq;
using Microsoft.EntityFrameworkCore;
using QueryBuilder.Core.Services;
using QueryBuilder.EFCore.SqlServer.Helpers;
using QueryBuilder.SqlServer;

namespace QueryBuilder.EFCore.SqlServer.Factories
{
    public class ServiceFactory<TEntity>
        : ISQLServiceFactory<TEntity>
        where TEntity : class
    {
        private readonly SQLServiceFactory<TEntity> _sqlServiceFactory;

        public ServiceFactory(IQueryable<TEntity> queryable)
        {
            DbContext dbContext = IQueryableHelper.GetDbContext(queryable);

            _sqlServiceFactory = CreateSqlServiceFactory(dbContext);
        }

        public IBulkInsertService<TEntity> CreateBulkInsert() => _sqlServiceFactory.CreateBulkInsert();
        public IBulkMergeService<TEntity> CreateBulkMerge() => _sqlServiceFactory.CreateBulkMerge();
        public ICommandService<TEntity> CreateCommandService() => _sqlServiceFactory.CreateCommandService();

        private SQLServiceFactory<TEntity> CreateSqlServiceFactory(DbContext dbContext)
        {
            var databaseContextFactory = new DatabaseAdapterFactory<TEntity>(dbContext);
            var mappingAdapterFactory = new MappingAdapterFactory<TEntity>(dbContext);

            return new SQLServiceFactory<TEntity>(
                commandProcessorFactory: databaseContextFactory,
                databaseContextFactory : databaseContextFactory,
                mappingAdapterFactory  : mappingAdapterFactory
            );
        }

    }
}
