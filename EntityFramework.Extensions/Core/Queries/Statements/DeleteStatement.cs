using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Core.Queries.Statements
{
    public class DeleteStatement<T>
    {
        public string TableName { get; private set; }

        public Expression<Func<T, bool>> Predicate { get; private set; }

        public DeleteStatement(string tableName, Expression<Func<T, bool>> predicate)
        {
            TableName = tableName;
            Predicate = predicate;
        }
    }
}
