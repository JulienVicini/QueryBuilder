using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.SqlServer.Bulk;
using QueryBuilder.SqlServer.Bulk.DataReader;
using System.Data.SqlClient;
using System.Linq;

namespace QueryBuilder.EF6.SqlServer.Factories
{
    public class BulkFacadeFactory<TRecord>
        where TRecord : class
    {
        private readonly DatabaseAdapterFactory<TRecord> _databaseAdapterFactory;
        private readonly MappingAdapterFactory<TRecord> _mappingFactory;

        public BulkFacadeFactory()
        {
            _databaseAdapterFactory = new DatabaseAdapterFactory<TRecord>();
            _mappingFactory         = new MappingAdapterFactory<TRecord>();
        }

        public BulkService<TRecord, IBulkDataReader> CreateBulkCopy(IQueryable<TRecord> queryable)
        {
            IDatabaseContext<SqlConnection, SqlTransaction> databaseContext
                = _databaseAdapterFactory.CreateDatabaseContext(queryable);

            IBulkExecutor<IBulkDataReader> executor
                = new SqlBulkCopyExecutor(databaseContext);

            return CreateBulkFacade(queryable, executor);
        }

        public BulkService<TRecord, IBulkDataReader> CreateBulkFacade(IQueryable<TRecord> queryable, IBulkExecutor<IBulkDataReader> executor)
        {
            IMappingAdapter<TRecord> mappingAdapter
                = _mappingFactory.CreateMappingAdapter(queryable);

            return new BulkService<TRecord, IBulkDataReader>(
                bulkExecutor   : executor,
                // TOTO select which strategy is better dataTransformer: new DataTableDataTransformer<TRecord>(mappingAdapter),
                dataTransformer: new DataReaderDataTransformer<TRecord>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );
        }
    }
}
