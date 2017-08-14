using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.Sql
{
    public class SqlContext : ISqlContext
    {
        public ObjectContext ObjectContext { get; private set; }

        public SqlContext(ObjectContext objectContext)
        {
            ObjectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        public SqlConnection GetConnection()
        {
            return ((EntityConnection)ObjectContext.Connection).StoreConnection as SqlConnection;
        }

        public int ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            return ObjectContext.ExecuteStoreCommand(query, parameters);
        }
    }
}
