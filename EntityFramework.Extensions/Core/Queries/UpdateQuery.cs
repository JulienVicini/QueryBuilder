using EntityFramework.Extensions.Core.Queries.Statements;
using EntityFramework.Extensions.Core.Queries.Statements.Operators;
using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Queries
{
    public class UpdateQuery<TEntity>
        where TEntity : class
    {
        public string TableName { get; private set; }

        public IEnumerable<AssignStatement<TEntity>> Assignements { get; private set; }

        public IFilterStatement Filter { get; private set; }

        public UpdateQuery(string tableName, IEnumerable<AssignStatement<TEntity>> assignements, IFilterStatement filter)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException(nameof(tableName));
            ThrowHelper.ThrowIfNullOrEmpty(assignements, nameof(assignements));

            TableName    = tableName;
            Assignements = assignements;
            Filter       = filter;
        }
    }
}
