using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Services;
using QueryBuilder.SqlServer.Bulk;
using QueryBuilder.SqlServer.Bulk.DataReader;
using QueryBuilder.SqlServer.Statements;

namespace QueryBuilder.SqlServer
{
    public class SQLServiceFactory<TEntity>
        : ISQLServiceFactory<TEntity>
        where TEntity : class
    {
        private readonly ISQLCommandProcessorFactory _commandProcessorFactory;
        private readonly ISQLDatabaseContextFactory _databaseContextFactory;
        private readonly IMappingAdapterFactory<TEntity> _mappingAdapterFactory;

        public SQLServiceFactory(ISQLCommandProcessorFactory commandProcessorFactory, ISQLDatabaseContextFactory databaseContextFactory, IMappingAdapterFactory<TEntity> mappingAdapterFactory)
        {
            _commandProcessorFactory = commandProcessorFactory ?? throw new ArgumentNullException(nameof(commandProcessorFactory));
            _databaseContextFactory  = databaseContextFactory  ?? throw new ArgumentNullException(nameof(databaseContextFactory));
            _mappingAdapterFactory   = mappingAdapterFactory   ?? throw new ArgumentNullException(nameof(mappingAdapterFactory));
        }

        public IBulkInsertService<TEntity> CreateBulkInsert()
        {
            IDatabaseContext<SqlConnection, SqlTransaction> databaseContext = _databaseContextFactory.Create();
            IMappingAdapter<TEntity> mappingAdapter = _mappingAdapterFactory.Create();

            return CreateBulkInsert(databaseContext, mappingAdapter);
        }

        public IBulkMergeService<TEntity> CreateBulkMerge()
        {
            ICommandProcessing<SqlTransaction> commandProcessor = _commandProcessorFactory.Create();
            IDatabaseContext<SqlConnection, SqlTransaction> databaseContext = _databaseContextFactory.Create();
            IMappingAdapter<TEntity> mappingAdapter = _mappingAdapterFactory.Create();

            IBulkInsertService<TEntity> bulkInsert = CreateBulkInsert(databaseContext, mappingAdapter);
            ICommandService<TEntity> commandService = CreateCommandService(commandProcessor, mappingAdapter);

            return new BulkMergeService<TEntity>(
                bulkInsertService: bulkInsert,
                commandService   : commandService
            );
        }

        public ICommandService<TEntity> CreateCommandService()
        {
            ICommandProcessing<SqlTransaction> commandProcessor = _commandProcessorFactory.Create();
            IMappingAdapter<TEntity> mappingAdapter = _mappingAdapterFactory.Create();

            return CreateCommandService(commandProcessor, mappingAdapter);
        }

        private IBulkInsertService<TEntity> CreateBulkInsert(IDatabaseContext<SqlConnection, SqlTransaction> databaseContext, IMappingAdapter<TEntity> mappingAdapter)
        {
            IDataTransformer<IEnumerable<TEntity>, IBulkDataReader> datatransformer
                = new DataReaderDataTransformer<TEntity>(mappingAdapter);

            return new BulkInsertService<TEntity, IBulkDataReader>(
                bulkInsert     : new SqlBulkInsert(databaseContext),
                dataTransformer: datatransformer,
                mappingAdapter : mappingAdapter
            );
        }

        private ICommandService<TEntity> CreateCommandService(ICommandProcessing<SqlTransaction> commandProcessor, IMappingAdapter<TEntity> mappingAdapter)
        {
            var queryTranslator = new SqlQueryTranslator<TEntity>(mappingAdapter);

            return new CommandService<TEntity, SqlTransaction>(
                queryTranslator: queryTranslator,
                queryProcessor : commandProcessor
            );
        }
    }
}
