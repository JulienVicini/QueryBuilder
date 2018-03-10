using System;
using System.Collections.Generic;
using System.Data;

namespace QueryBuilder.SqlServer.Bulk.DataReader
{
    public interface IBulkDataReader : IDataReader
    {
        /// <summary>
        /// The ordinal and name of the columns
        /// </summary>
        IReadOnlyCollection<Tuple<int, string>> Columns { get; }
    }
}
