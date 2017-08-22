using System.Collections.Generic;

namespace QueryBuilder.EntityFramework.Extensions.Core.Database
{
    public interface ICommandProcessing
    {
        int ExecuteCommand(string query, IEnumerable<object> parameters);
    }
}
