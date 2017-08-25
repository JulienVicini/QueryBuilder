using QueryBuilder.Core.Database;
using QueryBuilder.EntityFramework.Database;
using QueryBuilder.EntityFramework.IQueryable;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace QueryBuilder.EntityFramework.SqlServer.Factories
{
    public class DatabaseAdapterFactory<T>
        where T : class
    {
        public ICommandProcessing CreateCommandProcessor(IQueryable<T> queryable) => CreateObjectContextAdapter(queryable);
        public IDatabaseContext<SqlConnection, SqlTransaction> CreateDatabaseContext(IQueryable<T> queryable) => CreateObjectContextAdapter(queryable);

        public ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction> CreateObjectContextAdapter(IQueryable<T> queryable)
        {
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            return new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(objectContext);
        }
    }
}
