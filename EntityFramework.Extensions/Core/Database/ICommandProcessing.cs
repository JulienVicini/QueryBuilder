using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Database
{
    public interface ICommandProcessing
    {
        int ExecuteCommand(string query, IEnumerable<object> parameters);
    }
}
