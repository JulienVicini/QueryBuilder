using QueryBuilder.Core.Database;
using QueryBuilder.EF6.Database;
using QueryBuilder.EF6.Helpers;
using QueryBuilder.SqlServer;
using System;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace QueryBuilder.EF6.SqlServer.Factories
{
    public class DatabaseAdapterFactory<T>
        : ISQLCommandProcessorFactory, ISQLDatabaseContextFactory
        where T : class
    {
        private readonly ObjectContext _objectContext;

        public DatabaseAdapterFactory(ObjectContext objectContext)
        {
            _objectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        ICommandProcessing<SqlTransaction> ISQLCommandProcessorFactory.Create() => CreateObjectContextAdapter();
        IDatabaseContext<SqlConnection, SqlTransaction> ISQLDatabaseContextFactory.Create() => CreateObjectContextAdapter();

        private ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction> CreateObjectContextAdapter()
        {
            return new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(_objectContext);
        }
    }
}
