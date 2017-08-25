using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.SqlServer.Bulk;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QueryBuilder.EntityFramework.SqlServer.Factories
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

        public BulkFacade<TRecord, DataTable> CreateBulkCopy(IQueryable<TRecord> queryable)
        {
            IDatabaseContext<SqlConnection, SqlTransaction> databaseContext
                = _databaseAdapterFactory.CreateDatabaseContext(queryable);

            IBulkExecutor<DataTable> executor
                = new SqlBulkCopyExecutor(databaseContext);

            return CreateBulkFacade(queryable, executor);
        }

        public BulkFacade<TRecord, DataTable> CreateBulkFacade(IQueryable<TRecord> queryable, IBulkExecutor<DataTable> executor)
        {
            IMappingAdapter<TRecord> mappingAdapter
                = _mappingFactory.CreateMappingAdapter(queryable);

            return new BulkFacade<TRecord, DataTable>(
                bulkExecutor   : executor,
                dataTransformer: new DataTableDataTransformer<TRecord>(mappingAdapter),
                mappingAdapter : mappingAdapter
            );
        }
    }
}
