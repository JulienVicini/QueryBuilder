using QueryBuilder.Core.Helpers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder.Core.Statements
{
    public class MergeStatement<TEntity>
        where TEntity : class
    {
        public enum MergeType
        {
            InsertOnly,
            UpdateOnly,
            InsertOrUpdate
        }

        public IEnumerable<MemberExpression> Keys { get; }

        public string TemporaryTableName { get; }

        public MergeType Type { get; }

        public MergeStatement(IEnumerable<MemberExpression> keys, string temporaryTableName, MergeType mergeType)
        {
            Check.NotNullOrEmpty(keys, nameof(keys));
            Check.NotNullOrWhiteSpace(temporaryTableName, nameof(temporaryTableName));

            Keys               = keys;
            TemporaryTableName = temporaryTableName;
            Type               = mergeType;

        }
    }
}
