using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder.Core.Services
{
    public interface IBulkMergeService<TRecord>
    {
        void WriteToServer(IEnumerable<TRecord> records, IEnumerable<MemberExpression> mergeKey, bool updateOnly);
    }
}
