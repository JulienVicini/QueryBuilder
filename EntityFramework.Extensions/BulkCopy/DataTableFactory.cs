using EntityFramework.Extensions.Helpers;
using EntityFramework.Extensions.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EntityFramework.Extensions.BulkCopy
{
    public class DataTableFactory<T> where T : class
    {
        public DataTable CreateBulkInsertDataTable( IEnumerable<ColumnMapping> mappings )
        {
            if (mappings == null || !mappings.Any())
                throw new ArgumentException($"The parameter \"{nameof(mappings)}\" must contains at least one element");

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

        public DataRow CreateDataRow(DataTable dataTable, T record, IDictionary<string, PropertyInfo> mappedProperties)
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

        public IEnumerable<DataRow> CreateDataRows(DataTable dataTable, IEnumerable<T> records, IEnumerable<ColumnMapping> mappedColumns)
        {
            // Validate Input
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));

            ThrowHelper.ThrowIfNullOrEmpty(records      , nameof(records)      );
            ThrowHelper.ThrowIfNullOrEmpty(mappedColumns, nameof(mappedColumns));

            // Create Properties Accessor
            Type recordType = typeof(T);

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
