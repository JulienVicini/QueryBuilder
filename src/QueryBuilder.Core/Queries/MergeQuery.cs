using QueryBuilder.Helpers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder.Core.Queries
{
    public class MergeQuery<TEntity>
        where TEntity : class
    {
        public enum MergeType
        {
            InsertOnly,
            UpdateOnly,
            InsertOrUpdate
        }

        public IEnumerable<MemberExpression> Keys { get; private set; }

        public string TemporaryTableName { get; private set; }

        public MergeType Type { get; private set; }

        public MergeQuery(IEnumerable<MemberExpression> keys, string temporaryTableName, MergeType mergeType)
        {
            ThrowHelper.ThrowIfNullOrEmpty(keys, nameof(keys));
            ThrowHelper.ThrowIfNullOrWhiteSpace(temporaryTableName, nameof(temporaryTableName));

            Keys               = keys;
            TemporaryTableName = temporaryTableName;
            Type               = mergeType;

        }
    }
}
