using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using QueryBuilder.Core.Database;

namespace QueryBuilder.Core.Services
{
    public class BulkInsertService<TData, TBulkData>
        : IBulkInsertService<TData>
        where TData : class
        where TBulkData : class
    {
        private readonly IBulkInsert<TBulkData> _bulkInsert;
        private readonly IDataTransformer<IEnumerable<TData>, TBulkData> _dataTransformer;
        private readonly IMappingAdapter<TData> _mappingAdapter;

        public BulkInsertService(IBulkInsert<TBulkData> bulkInsert, IDataTransformer<IEnumerable<TData>, TBulkData> dataTransformer, IMappingAdapter<TData> mappingAdapter)
        {
            _bulkInsert      = bulkInsert      ?? throw new ArgumentNullException(nameof(bulkInsert));
            _dataTransformer = dataTransformer ?? throw new ArgumentNullException(nameof(dataTransformer));
            _mappingAdapter  = mappingAdapter  ?? throw new ArgumentNullException(nameof(mappingAdapter));
        }

        public void WriteToServer(IEnumerable<TData> records)
        {
            string tableName = _mappingAdapter.GetTableName();

            WriteToServer(records, tableName);
        }

        public void WriteToServer(IEnumerable<TData> records, string tableName)
        {
            Check.NotNullOrEmpty(records, nameof(records));
            Check.NotNullOrWhiteSpace(tableName, nameof(tableName));
            
            // Transform records
            TBulkData bulkData = _dataTransformer.Transform(records);

            // Insert Data
            _bulkInsert.Write(
                records  : bulkData,
                tableName: tableName
            );

            // Dipose bulkData if needed
            (bulkData as IDisposable)?.Dispose();
        }
    }
}
