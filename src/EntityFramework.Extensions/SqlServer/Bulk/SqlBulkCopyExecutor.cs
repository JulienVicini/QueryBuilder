using EntityFramework.Extensions.Core.Bulk;
using EntityFramework.Extensions.Core.Database;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.SqlServer.Bulk
{
    public class SqlBulkCopyExecutor
        : IBulkExecutor<DataTable>
    {
        protected readonly IDatabaseContext _sqlContext;

        public SqlBulkCopyExecutor(IDatabaseContext sqlContext)
        {
            _sqlContext = sqlContext ?? throw new ArgumentNullException(nameof(sqlContext));
        }

        public void Write(string tableName, DataTable records)
        {
            // Perform Bulk Copy
            SqlConnection sqlConn = ((EntityConnection)_sqlContext.GetConnection()).StoreConnection as SqlConnection; // Todo put this logic elsewhere

            if(sqlConn.State != ConnectionState.Open)
                sqlConn.Open();

            DbTransaction dbTransaction = null;

            try
            {
                dbTransaction = _sqlContext.BeginTransaction();
                SqlTransaction sqlTransaction = ((EntityTransaction)dbTransaction).StoreTransaction as SqlTransaction;


                Write(tableName, records, sqlConn, sqlTransaction);

                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction?.Rollback();
                throw;
            }
            finally
            {
                dbTransaction?.Dispose();
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
