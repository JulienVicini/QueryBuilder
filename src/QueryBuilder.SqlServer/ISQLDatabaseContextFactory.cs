using System;
using System.Data.SqlClient;
using QueryBuilder.Core.Database;

namespace QueryBuilder.SqlServer
{
    public interface ISQLDatabaseContextFactory
    {
        IDatabaseContext<SqlConnection, SqlTransaction> Create();
    }
}
