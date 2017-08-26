using QueryBuilder.Core.Bulk;
using QueryBuilder.Core.Database;
using QueryBuilder.SqlServer.Bulk.DataReader;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QueryBuilder.SqlServer.Bulk
{
    public class SqlBulkCopyExecutor
        : IBulkExecutor<IBulkDataReader>
    {
        protected readonly IDatabaseContext<SqlConnection, SqlTransaction> _sqlContext;

        public SqlBulkCopyExecutor(IDatabaseContext<SqlConnection, SqlTransaction> sqlContext)
        {
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
        }

        public void Write(string tableName, IBulkDataReader dataReader)
        {
            // Perform Bulk Copy
            SqlConnection sqlConn = _sqlContext.GetConnection();

            if(sqlConn.State != ConnectionState.Open)
                sqlConn.Open();


            using (SqlTransaction sqlTransaction = _sqlContext.BeginTransaction())
            {
                Write(tableName, dataReader, sqlConn, sqlTransaction);

                sqlTransaction.Commit();
            }
        }

        public virtual void Write(string tableName, IBulkDataReader dataReader, SqlConnection sqlConn, SqlTransaction transaction)
        {
            using (var bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                foreach (string columnName in dataReader.Columns)
                    bulkCopy.ColumnMappings.Add(columnName, columnName);

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataReader);
            }
        }
    }
}
