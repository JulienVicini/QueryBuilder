using System.Collections.Generic;

namespace QueryBuilder.Core.Database
{
    public interface ICommandProcessing
    {
        int ExecuteCommand(string query, IEnumerable<object> parameters);
    }
}
