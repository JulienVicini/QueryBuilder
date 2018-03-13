using QueryBuilder.Core.Database;
using QueryBuilder.EFCore.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;
using QueryBuilder.SqlServer;
using System;

namespace QueryBuilder.EFCore.SqlServer.Factories
{
    public class DatabaseAdapterFactory<T>
        : ISQLCommandProcessorFactory, ISQLDatabaseContextFactory
        where T : class
    {
        private readonly DbContext _dbContext;

        public DatabaseAdapterFactory(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        ICommandProcessing<SqlTransaction> ISQLCommandProcessorFactory.Create() => CreateObjectContextAdapter();
        IDatabaseContext<SqlConnection, SqlTransaction> ISQLDatabaseContextFactory.Create() => CreateObjectContextAdapter();

        private EFCoreContextAdapter<SqlConnection, SqlTransaction> CreateObjectContextAdapter()
        {
            DatabaseFacade databaseFace = _dbContext.Database;

            return new EFCoreContextAdapter<SqlConnection, SqlTransaction>(databaseFace);
        }
    }
}
