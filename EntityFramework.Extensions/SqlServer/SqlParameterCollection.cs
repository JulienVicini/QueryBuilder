using EntityFramework.Extensions.Core.Queries;
using System.Data.SqlClient;
using System.Linq;

namespace EntityFramework.Extensions.SqlServer
{
    public class SqlParameterCollection : ParameterCollection<SqlParameter>
    {
        public override string AddParameter(object value)
        {
            string parameterName = "@p" + _parameters.Count();

            _parameters.Add(
                new SqlParameter(parameterName, value)
            );

            return parameterName;
        }
    }
}
