using EntityFramework.Extensions.Core.Queries.Statements;
using System;

namespace EntityFramework.Extensions.Core.Queries
{
    public class DeleteQuery
    {
        public string TableName { get; private set; }

        public IFilterStatement Filter { get; private set; }

        public DeleteQuery(string tableName, IFilterStatement filter)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException(nameof(tableName));

            TableName = tableName;
            Filter    = filter;
        }
    }
}
