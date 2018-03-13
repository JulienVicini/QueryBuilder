using QueryBuilder.Core.Database;
using QueryBuilder.SqlServer.Bulk.DataReader;
using System;
using System.Data;
using System.Data.SqlClient;

namespace QueryBuilder.SqlServer.Bulk
{
    public class SqlBulkInsert
        : IBulkInsert<IBulkDataReader>
    {
        protected readonly IDatabaseContext<SqlConnection, SqlTransaction> _sqlContext;

        public SqlBulkInsert(IDatabaseContext<SqlConnection, SqlTransaction> sqlContext)
        {
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
        }

        public void Write(string tableName, IBulkDataReader dataReader)
        {
            // Perform Bulk Copy
            SqlConnection sqlConn = _sqlContext.GetConnection();

            if(sqlConn.State != ConnectionState.Open)
                sqlConn.Open();

            using (ITransactionScope<SqlTransaction> sqlTransaction = _sqlContext.BeginTransaction())
            {
                Write(tableName, dataReader, sqlConn, sqlTransaction.Current);
                sqlTransaction.Commit();
            }
        }

        public virtual void Write(string tableName, IBulkDataReader dataReader, SqlConnection sqlConn, SqlTransaction transaction)
        {
            using (var bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                foreach (Tuple<int, string> column in dataReader.Columns)
                {
                    // use ordinal when adding mapping instead of column name
                    // bug will be solved in Standard Library 2.1 https://github.com/dotnet/corefx/pull/24655
                    bulkCopy.ColumnMappings.Add(column.Item1, column.Item2);
                }

                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataReader);
            }
        }
    }
}
