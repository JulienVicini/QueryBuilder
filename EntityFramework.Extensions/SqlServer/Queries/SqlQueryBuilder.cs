using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public class SqlQueryBuilder
    {
        public StringBuilder Query { get; private set; }

        public SqlParameterCollection ParameterCollection { get; private set; }

        public SqlQueryBuilder()
        {
            Query               = new StringBuilder();
            ParameterCollection = new SqlParameterCollection();
        }

        public void AppendValue(object value)
        {
            string parameterName = ParameterCollection.AddParameter(value);

            Query.Append(parameterName);
        }

        // TODO suppress that
        public (string query, IEnumerable<object> parameters) Build()
        {
            return (
                Query.ToString(),
                ParameterCollection.Parameters
            );
        }

        public void Deconstruct(out string query, out IEnumerable<SqlParameter> parameters)
        {
            query      = Query.ToString();
            parameters = ParameterCollection.Parameters;
        }
    }
}
