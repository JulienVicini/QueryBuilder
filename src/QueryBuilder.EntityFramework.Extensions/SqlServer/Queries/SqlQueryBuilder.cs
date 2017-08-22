using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public class SqlQueryBuilder
    {
        private readonly StringBuilder _query;

        private List<SqlParameter> _parameters;

        public SqlQueryBuilder()
        {
            _query      = new StringBuilder();
            _parameters = new List<SqlParameter>();
        }

        public SqlQueryBuilder AppendParameterValue(object paremeterValue)
        {
            string parameterName = "@p" + _parameters.Count;

            _parameters.Add(
                new SqlParameter(parameterName, paremeterValue)
            );

            return Append(parameterName);
        }

        public SqlQueryBuilder Append(string text)
        {
            _query.Append(text);
            return this;
        }

        // TODO suppress that
        public (string query, IEnumerable<object> parameters) Build()
        {
            return (
                _query.ToString(),
                _parameters
            );
        }

        public void Deconstruct(out string query, out IEnumerable<SqlParameter> parameters)
        {
            query      = _query.ToString();
            parameters = _parameters;
        }
    }
}
