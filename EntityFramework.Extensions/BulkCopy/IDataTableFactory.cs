using EntityFramework.Extensions.Mappings;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace EntityFramework.Extensions.BulkCopy
{
    public interface IDataTableFactory<T>
        where T : class
    {
        DataTable CreateBulkInsertDataTable(IEnumerable<ColumnMapping> mappings);

        DataRow CreateDataRow(DataTable dataTable, T record, IDictionary<string, PropertyInfo> mappedProperties);

        IEnumerable<DataRow> CreateDataRows(DataTable dataTable, IEnumerable<T> records, IEnumerable<ColumnMapping> mappedColumns);
    }
}
