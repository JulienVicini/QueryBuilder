using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.BulkCopy
{
    public class BulkCopy<TData, TBulkData>
        where TData : class
        where TBulkData : class
    {
        private readonly IBulkCopyExecutor<TBulkData> _bulkCopyExcecutor;
        private readonly IDataTransformer<IEnumerable<TData>, TBulkData> _dataTransformer;
        private readonly IMappingAdapter<TData> _mappingAdapter;

        public BulkCopy(IBulkCopyExecutor<TBulkData> bulkCopyExecutor, IDataTransformer<IEnumerable<TData>, TBulkData> dataTransformer, IMappingAdapter<TData> mappingAdapter)
        {
            _bulkCopyExcecutor = bulkCopyExecutor ?? throw new ArgumentNullException(nameof(bulkCopyExecutor));
            _dataTransformer   = dataTransformer  ?? throw new ArgumentNullException(nameof(dataTransformer));
            _mappingAdapter    = mappingAdapter   ?? throw new ArgumentNullException(nameof(mappingAdapter));
        }

        public void WriteToServer(IEnumerable<TData> records)
        {
            ThrowHelper.ThrowIfNullOrEmpty(records, nameof(records));
            
            // Transform records
            TBulkData bulkData = _dataTransformer.Transform(records);

            // Insert Data
            _bulkCopyExcecutor.Write(
                tableName: _mappingAdapter.GetTableName(),
                records  : bulkData
            );
        }
    }
}
