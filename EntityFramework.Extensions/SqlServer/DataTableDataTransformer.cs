using EntityFramework.Extensions.Core.BulkCopy;
using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EntityFramework.Extensions.SqlServer
{
    public class DataTableDataTransformer<TEntity>
        : IDataTransformer<IEnumerable<TEntity>, DataTable>
        where TEntity : class
    {
        private readonly IMappingAdapter<TEntity> _mappingAdapter;

        public DataTableDataTransformer(IMappingAdapter<TEntity> mappingAdapter)
        {
            _mappingAdapter = mappingAdapter ?? throw new ArgumentNullException(nameof(mappingAdapter));
        }

        public DataTable Transform(IEnumerable<TEntity> sourceData)
        {
            ThrowHelper.ThrowIfNullOrEmpty(sourceData, nameof(sourceData));

            DataTable dataTable = CreateDataTable();

            foreach (DataRow row in CreateDataRows(dataTable, sourceData))
                dataTable.Rows.Add(row);

            return dataTable;
        }

        public DataTable CreateDataTable()
        {
            IEnumerable<ColumnMapping> mappings = GetMappedColumns();

            DataTable dataTable = new DataTable();

            try
            {
                foreach (ColumnMapping mapping in mappings.Where(r => !r.IsIdentity))
                    dataTable.Columns.Add(mapping.SqlName, mapping.PropertyType);
            }
            catch (DuplicateNameException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            return dataTable;
        }

        public IEnumerable<ColumnMapping> GetMappedColumns()
        {
            IEnumerable<ColumnMapping> mappings = _mappingAdapter.GetColumns();

            if (mappings == null || !mappings.Any())
                throw new InvalidOperationException($"The entity \"{typeof(TEntity)}\" must have at least one mapped property");
            return mappings;
        }

        public DataRow CreateDataRow(DataTable dataTable, TEntity record, IDictionary<string, PropertyInfo> mappedProperties)
        {
            if (dataTable == null) throw new ArgumentNullException( nameof(dataTable) );
            if (record    == null) throw new ArgumentNullException( nameof(record)    );
            if (mappedProperties == null || !mappedProperties.Any())
                throw new ArgumentException($"The parameter \"{nameof(mappedProperties)}\" must contains at least one mapped property.");

            DataRow row = dataTable.NewRow();

            foreach(KeyValuePair<string, PropertyInfo> property in mappedProperties)
                row[property.Key] = property.Value.GetValue( record );

            return row;
        }

        public IEnumerable<DataRow> CreateDataRows(DataTable dataTable, IEnumerable<TEntity> records)
        {
            // Validate Input
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));

            ThrowHelper.ThrowIfNullOrEmpty(records      , nameof(records)      );

            IEnumerable<ColumnMapping> mappedColumns = GetMappedColumns();


            // Create Properties Accessor
            Type recordType = typeof(TEntity);

            // Todo Refactor this
            Dictionary<string, PropertyInfo> mappedProperties
                = mappedColumns.Where(mc => !mc.IsIdentity)
                               .ToDictionary(
                                    c => c.SqlName,
                                    c => recordType.GetProperty(c.PropertyName)
                                );

            return records.Select(r =>
                CreateDataRow(dataTable, r, mappedProperties)
            );
        }
    }
}
