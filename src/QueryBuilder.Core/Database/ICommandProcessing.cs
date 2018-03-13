using System.Collections.Generic;

namespace QueryBuilder.Core.Database
{
    public interface ICommandProcessing<TTransaction>
    {
        int ExecuteCommand(string query, IEnumerable<object> parameters);

        int ExecuteCommand(string query, IEnumerable<object> parameters, TTransaction transaction);
    }
}
