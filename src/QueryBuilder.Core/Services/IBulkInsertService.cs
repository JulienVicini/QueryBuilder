using System;
using System.Collections.Generic;
using System.Text;

namespace QueryBuilder.Core.Services
{
    public interface IBulkInsertService<TRecord>
    {
        void WriteToServer(IEnumerable<TRecord> records);

        void WriteToServer(IEnumerable<TRecord> records, string tableName);
    }
}
