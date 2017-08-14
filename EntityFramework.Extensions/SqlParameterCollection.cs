using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace EntityFramework.Extensions
{
    public class SqlParameterCollection
    {
        private readonly List<SqlParameter> _parameters;

        public IEnumerable<SqlParameter> Parameters { get => _parameters.AsReadOnly(); }

        public SqlParameterCollection()
        {
            _parameters = new List<SqlParameter>();
        }

        public string AddParameter<T>(T value)
        {
            string parameterName = "@p" + _parameters.Count();

            _parameters.Add(
                new SqlParameter(parameterName, value )
            );

            return parameterName;
        }
    }
}
