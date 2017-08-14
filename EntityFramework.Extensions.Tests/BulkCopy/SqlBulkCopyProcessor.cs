using EntityFramework.Extensions.BulkCopy;
using EntityFramework.Extensions.Mappings;
using EntityFramework.Extensions.Sql;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.Tests.BulkCopy
{
    public class SqlBulkCopyProcessor<TEntity>
        where TEntity : class
    {
        private readonly IDataTableFactory<TEntity> _dataTableFactory;
        private readonly ISqlContext<TEntity> _sqlContext;

        public SqlBulkCopyProcessor(IDataTableFactory<TEntity> dataTableFactory, ISqlContext<TEntity> sqlContext)
        {
            _dataTableFactory = dataTableFactory;
            _sqlContext       = sqlContext;
        }

        public void Execute(IEnumerable<TEntity> records)
        {
            IEnumerable<ColumnMapping> mappings = _sqlContext.GetMappings();

            using(DataTable dataTable = _dataTableFactory.CreateBulkInsertDataTable(mappings))
            {
                foreach (DataRow row in _dataTableFactory.CreateDataRows(dataTable, records, mappings))
                    dataTable.Rows.Add(row);

                // Perform Bulk Copy
                SqlConnection connection = _sqlContext.GetConnection();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
