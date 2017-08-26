using System.Collections.Generic;
using System.Data;

namespace QueryBuilder.SqlServer.Bulk.DataReader
{
    public interface IBulkDataReader : IDataReader
    {
        IReadOnlyCollection<string> Columns { get; }
    }
}
