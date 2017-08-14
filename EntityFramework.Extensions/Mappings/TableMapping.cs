using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Mappings
{
    public class TableMapping
    {
        public string FullTableName {
            get => $"[{Schema}].[{TableName}]";
        }

        public string TableName { get; private set; }

        public string Schema { get; private set; }

        public IEnumerable<ColumnMapping> Columns { get; private set; }

        public TableMapping(string tableName, string schema, IEnumerable<ColumnMapping> columns)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Schema    = schema    ?? throw new ArgumentNullException(nameof(schema));

            ThrowHelper.ThrowIfNullOrEmpty(columns, nameof(columns));
            Columns = columns;
        }
    }
}
