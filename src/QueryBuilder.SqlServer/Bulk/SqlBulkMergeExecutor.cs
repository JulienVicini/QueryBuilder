using QueryBuilder.Core.Database;
using QueryBuilder.Core.Statements;
using QueryBuilder.SqlServer.Bulk.DataReader;
using System;
using System.Data.SqlClient;

namespace QueryBuilder.SqlServer.Bulk
{
    public class SqlBulkMergeExecutor<TEntity> : SqlBulkCopyExecutor
        where TEntity : class
    {
        private readonly StatementFacade<TEntity> _queryOrchestrator;

        private readonly MergeStatement<TEntity> _mergeQuery;

        public SqlBulkMergeExecutor(IDatabaseContext<SqlConnection, SqlTransaction> sqlContext, StatementFacade<TEntity> queryOrchestrator, MergeStatement<TEntity> mergeQuery)
            : base(sqlContext)
        {
            _queryOrchestrator = queryOrchestrator ?? throw new ArgumentNullException(nameof(queryOrchestrator));
            _mergeQuery        = mergeQuery        ?? throw new ArgumentNullException(nameof(mergeQuery));
        }

        public override void Write(string tableName, IBulkDataReader dataReader, SqlConnection sqlConn, SqlTransaction transaction)
        {
            // Create Temporary Table
            DropCreateTemporaryTable(AlterTableStatement<TEntity>.AlterType.Create);

            // Insert Data
            base.Write(_mergeQuery.TemporaryTableName, dataReader, sqlConn, transaction);

            // Execute merge statement
            _queryOrchestrator.Merge(_mergeQuery);

            // Drop #bulkTmp
            DropCreateTemporaryTable(AlterTableStatement<TEntity>.AlterType.Drop);
        }

        public void DropCreateTemporaryTable(AlterTableStatement<TEntity>.AlterType alterType)
        {
            _queryOrchestrator.Alter(
                new AlterTableStatement<TEntity>(_mergeQuery.TemporaryTableName, alterType)
            );
        }
    }
}
