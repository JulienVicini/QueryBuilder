using System.Data.SqlClient;

namespace EntityFramework.Extensions.Sql
{
    public interface ISqlContext
    {
        SqlConnection GetConnection();

        int ExecuteQuery(string query, params SqlParameter[] parameters);
    }
}
