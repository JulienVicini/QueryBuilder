using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using QueryBuilder.Core.IQueryables;
using QueryBuilder.Core.Services;
using QueryBuilder.EF6.Helpers;
using QueryBuilder.SqlServer;

namespace QueryBuilder.EF6.SqlServer.Factories
{
    public class ServiceFactory<TEntity>
        : ISQLServiceFactory<TEntity>
        where TEntity : class
    {
        private readonly SQLServiceFactory<TEntity> _sqlServiceFactory;

        public ServiceFactory(IQueryable<TEntity> queryable)
        {
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            _sqlServiceFactory = CreateSqlServiceFactory(objectContext);
        }

        public IBulkInsertService<TEntity> CreateBulkInsert() => _sqlServiceFactory.CreateBulkInsert();
        public IBulkMergeService<TEntity> CreateBulkMerge() => _sqlServiceFactory.CreateBulkMerge();
        public ICommandService<TEntity> CreateCommandService() => _sqlServiceFactory.CreateCommandService();

        private SQLServiceFactory<TEntity> CreateSqlServiceFactory(ObjectContext objectContext)
        {
            var databaseContextFactory = new DatabaseAdapterFactory<TEntity>(objectContext);
            var mappingAdapterFactory  = new MappingAdapterFactory<TEntity>(objectContext);

            return new SQLServiceFactory<TEntity>(
                commandProcessorFactory: databaseContextFactory,
                databaseContextFactory : databaseContextFactory,
                mappingAdapterFactory  : mappingAdapterFactory
            );
        }

    }
}
