using EntityFramework.Extensions.Core.BulkCopy;
using EntityFramework.Extensions.Core.Database;
using System;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.SqlServer
{
    public class SqlBulkCopyExecutor
        : IBulkCopyExecutor<DataTable>
    {
        private readonly IDatabaseContext _sqlContext;

        public SqlBulkCopyExecutor(IDatabaseContext sqlContext)
        {
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
        }

        public void Write(string tableName, DataTable records)
        {
            // Perform Bulk Copy
            string connectionString = ((EntityConnection)_sqlContext.GetConnection()).StoreConnection.ConnectionString; // Todo put this logic elsewhere

            using (var sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();

                using (var bulkCopy = new SqlBulkCopy(sqlConn))
                {
                    foreach (DataColumn column in records.Columns)
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);

                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(records);
                }
            }
        }
    }
}
