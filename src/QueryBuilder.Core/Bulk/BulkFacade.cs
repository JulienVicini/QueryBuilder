using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Core.Bulk
{
    public class BulkFacade<TData, TBulkData>
        where TData : class
        where TBulkData : class
    {
        private readonly IBulkExecutor<TBulkData> _bulkExcecutor;
        private readonly IDataTransformer<IEnumerable<TData>, TBulkData> _dataTransformer;
        private readonly IMappingAdapter<TData> _mappingAdapter;

        public BulkFacade(IBulkExecutor<TBulkData> bulkExecutor, IDataTransformer<IEnumerable<TData>, TBulkData> dataTransformer, IMappingAdapter<TData> mappingAdapter)
        {
            _bulkExcecutor   = bulkExecutor    ?? throw new ArgumentNullException(nameof(bulkExecutor));
            _dataTransformer = dataTransformer ?? throw new ArgumentNullException(nameof(dataTransformer));
            _mappingAdapter  = mappingAdapter  ?? throw new ArgumentNullException(nameof(mappingAdapter));
        }

        public void WriteToServer(IEnumerable<TData> records)
        {
            ThrowHelper.ThrowIfNullOrEmpty(records, nameof(records));
            
            // Transform records
            TBulkData bulkData = _dataTransformer.Transform(records);

            // Insert Data
            _bulkExcecutor.Write(
                tableName: _mappingAdapter.GetTableName(),
                records  : bulkData
            );

            // Dipose bulkData if needed
            (bulkData as IDisposable)?.Dispose();
        }
    }
}
