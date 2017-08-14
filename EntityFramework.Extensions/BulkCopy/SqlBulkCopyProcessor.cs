using EntityFramework.Extensions.Mappings;
using EntityFramework.Extensions.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.BulkCopy
{
    public class SqlBulkCopyProcessor<TEntity>
        where TEntity : class
    {
        private readonly IDataTableFactory<TEntity> _dataTableFactory;
        private readonly ISqlContext _sqlContext;
        private readonly TableMapping _tableMapping;

        public SqlBulkCopyProcessor(IDataTableFactory<TEntity> dataTableFactory, ISqlContext sqlContext, TableMapping tableMapping)
        {
            _dataTableFactory = dataTableFactory ?? throw new ArgumentNullException(nameof(dataTableFactory));
            _sqlContext       = sqlContext       ?? throw new ArgumentNullException(nameof(sqlContext));
            _tableMapping     = tableMapping     ?? throw new ArgumentNullException(nameof(tableMapping));
        }

        public void Execute(IEnumerable<TEntity> records)
        {
            using(DataTable dataTable = _dataTableFactory.CreateBulkInsertDataTable(_tableMapping.Columns))
            {
                foreach (DataRow row in _dataTableFactory.CreateDataRows(dataTable, records, _tableMapping.Columns))
                    dataTable.Rows.Add(row);

                // Perform Bulk Copy
                using (SqlConnection connection = new SqlConnection(_sqlContext.GetConnection().ConnectionString))
                {
                    // Open connection if needed
                    //if (connection.State == ConnectionState.Closed)
                    connection.Open();

                    using (var bulkCopy = new SqlBulkCopy(connection))
                    {
                        foreach(DataColumn column in dataTable.Columns)
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);

                        bulkCopy.DestinationTableName = "[table].[user]"; //_tableMapping.FullTableName;
                        bulkCopy.WriteToServer(dataTable);
                    }

                    //connection.Dispose();
                }
            }
        }
    }
}
