using EntityFramework.Extensions.Core.Bulk;
using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            IEnumerable<ColumnMapping<TEntity>> mappings = GetMappedColumns();

            DataTable dataTable = new DataTable();

            try
            {
                foreach (ColumnMapping<TEntity> mapping in mappings.Where(r => !r.IsIdentity))
                    dataTable.Columns.Add(mapping.DbColumnName, mapping.PropertyInfo.PropertyType);
            }
            catch (DuplicateNameException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            return dataTable;
        }

        public IEnumerable<ColumnMapping<TEntity>> GetMappedColumns()
        {
            IEnumerable<ColumnMapping<TEntity>> mappings = _mappingAdapter.GetColumns();

            if (mappings == null || !mappings.Any())
                throw new InvalidOperationException($"The entity \"{typeof(TEntity)}\" must have at least one mapped property");
            return mappings;
        }

        public DataRow CreateDataRow(DataTable dataTable, TEntity record, IEnumerable<ColumnMapping<TEntity>> mappedProperties)
        {
            if (dataTable == null) throw new ArgumentNullException( nameof(dataTable) );
            if (record    == null) throw new ArgumentNullException( nameof(record)    );
            if (mappedProperties == null || !mappedProperties.Any())
                throw new ArgumentException($"The parameter \"{nameof(mappedProperties)}\" must contains at least one mapped property.");

            DataRow row = dataTable.NewRow();

            foreach(ColumnMapping<TEntity> mappedProperty in mappedProperties)
                row[mappedProperty.DbColumnName] = mappedProperty.PropertyInfo.GetValue( record );

            return row;
        }

        public IEnumerable<DataRow> CreateDataRows(DataTable dataTable, IEnumerable<TEntity> records)
        {
            // Validate Input
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));

            ThrowHelper.ThrowIfNullOrEmpty(records      , nameof(records)      );

            IEnumerable<ColumnMapping<TEntity>> mappedColumns = GetMappedColumns().Where(r => !r.IsIdentity);

            return records.Select(r =>
                CreateDataRow(dataTable, r, mappedColumns)
            );
        }
    }
}
