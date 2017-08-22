using EntityFramework.Extensions.Core.Database;
using EntityFramework.Extensions.Core.Queries;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EntityFramework.Extensions.SqlServer.Bulk
{
    public class SqlBulkMergeExecutor<TEntity> : SqlBulkCopyExecutor
        where TEntity : class
    {
        private readonly QueryOrchestrator<TEntity> _queryOrchestrator;

        private readonly MergeQuery<TEntity> _mergeQuery;

        public SqlBulkMergeExecutor(IDatabaseContext sqlContext, QueryOrchestrator<TEntity> queryOrchestrator, MergeQuery<TEntity> mergeQuery)
            : base(sqlContext)
        {
            _queryOrchestrator = queryOrchestrator ?? throw new ArgumentNullException(nameof(queryOrchestrator));
            _mergeQuery        = mergeQuery        ?? throw new ArgumentNullException(nameof(mergeQuery));
        }

        public override void Write(string tableName, DataTable records, SqlConnection sqlConn, SqlTransaction transaction)
        {
            // Create Temporary Table
            DropCreateTemporaryTable(AlterTableQuery<TEntity>.AlterType.Create);

            // Insert Data
            base.Write(_mergeQuery.TemporaryTableName, records, sqlConn, transaction);

            // Execute merge statement
            _queryOrchestrator.Merge(_mergeQuery);

            // Drop #bulkTmp
            DropCreateTemporaryTable(AlterTableQuery<TEntity>.AlterType.Drop);
        }

        public void DropCreateTemporaryTable(AlterTableQuery<TEntity>.AlterType alterType)
        {
            _queryOrchestrator.Alter(
                new AlterTableQuery<TEntity>(_mergeQuery.TemporaryTableName, alterType)
            );
        }
    }
}
