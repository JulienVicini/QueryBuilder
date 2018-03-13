using System.Data.SqlClient;
using QueryBuilder.Core.Database;

namespace QueryBuilder.SqlServer
{
    public interface ISQLCommandProcessorFactory
    {
        ICommandProcessing<SqlTransaction> Create();
    }
}
