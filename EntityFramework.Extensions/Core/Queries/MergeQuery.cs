using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Core.Queries
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
            if (string.IsNullOrWhiteSpace(temporaryTableName)) throw new ArgumentException(nameof(temporaryTableName));

            Keys               = keys;
            TemporaryTableName = temporaryTableName;
            Type               = mergeType;

        }
    }
}
