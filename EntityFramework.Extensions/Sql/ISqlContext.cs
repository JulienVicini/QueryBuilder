using EntityFramework.Extensions.Mappings;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.Sql
{
    public interface ISqlContext<TEntity>
        where TEntity : class
    {
        IEnumerable<ColumnMapping> GetMappings();

        SqlConnection GetConnection();

        int ExecuteQuery(string query, params SqlParameter[] parameters);
    }
}
