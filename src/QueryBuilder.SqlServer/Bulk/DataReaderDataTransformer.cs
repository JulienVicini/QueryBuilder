using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Mappings;
using QueryBuilder.SqlServer.Bulk.DataReader;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace QueryBuilder.SqlServer.Bulk
{
    public class DataReaderDataTransformer<TEntity>
        : IDataTransformer<IEnumerable<TEntity>, IBulkDataReader>
        where TEntity :  class
    {
        private readonly IMappingAdapter<TEntity> _mappingAdapter;

        public DataReaderDataTransformer(IMappingAdapter<TEntity> mappingAdapter)
        {
            _mappingAdapter = mappingAdapter;
        }

        public IBulkDataReader Transform(IEnumerable<TEntity> sourceData)
        {
            List<ColumnMapping<TEntity>> columns 
                = _mappingAdapter.GetColumns()
                                 .Where(c => !c.IsIdentity) // TODO move this elsewhere
                                 .ToList();

            return new LazyDataReader<TEntity>(
                sourceData,
                GetColumnNames(columns),
                GetLookup(columns)
            );
        }

        public OrdinalValueLookup<TEntity> GetLookup(List<ColumnMapping<TEntity>> columns)
        {
            var builder = new OrdinalValueLookupBuilder<TEntity>();

            foreach (ColumnMapping<TEntity> column in columns)
                builder.AddMapping(column);

            return builder.BuildOrdinalValueLookup();
        }

        private IReadOnlyCollection<string> GetColumnNames(List<ColumnMapping<TEntity>> columns)
        {
            return new ReadOnlyCollection<string>(
                columns.Select(c => c.DbColumnName)
                       .ToList()
            );
        }
    }
}
