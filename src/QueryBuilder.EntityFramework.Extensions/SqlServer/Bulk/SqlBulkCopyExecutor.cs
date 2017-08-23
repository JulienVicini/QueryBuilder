using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Database;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QueryBuilder.EntityFramework.Extensions.SqlServer.Bulk
{
    public class SqlBulkCopyExecutor
        : IBulkExecutor<DataTable>
    {
        protected readonly IDatabaseContext<SqlConnection, SqlTransaction> _sqlContext;

        public SqlBulkCopyExecutor(IDatabaseContext<SqlConnection, SqlTransaction> sqlContext)
        {
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
        }

        public void Write(string tableName, DataTable records)
        {
            // Perform Bulk Copy
            SqlConnection sqlConn = _sqlContext.GetConnection();

            if(sqlConn.State != ConnectionState.Open)
                sqlConn.Open();


            using (SqlTransaction sqlTransaction = _sqlContext.BeginTransaction())
            {
                Write(tableName, records, sqlConn, sqlTransaction);

                sqlTransaction.Commit();
            }
        }

        public virtual void Write(string tableName, DataTable records, SqlConnection sqlConn, SqlTransaction transaction)
        {
            using (var bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                foreach (DataColumn column in records.Columns)
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(records);
            }
        }
    }
}
