using QueryBuilder.Core.Database;
using QueryBuilder.EFCore.Database;
using QueryBuilder.EFCore.SqlServer.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace QueryBuilder.EFCore.SqlServer.Factories
{
    public class DatabaseAdapterFactory<T>
        where T : class
    {
        public ICommandProcessing CreateCommandProcessor(IQueryable<T> queryable) => CreateObjectContextAdapter(queryable);

        public IDatabaseContext<SqlConnection, SqlTransaction> CreateDatabaseContext(IQueryable<T> queryable) => CreateObjectContextAdapter(queryable);

        public EFCoreContextAdapter<SqlConnection, SqlTransaction> CreateObjectContextAdapter(IQueryable<T> queryable)
        {
            DbContext dbContext = IQueryableHelper.GetDbContext(queryable);

            DatabaseFacade databaseFace = dbContext.Database;


            return new EFCoreContextAdapter<SqlConnection, SqlTransaction>(databaseFace);
        }
    }
}
