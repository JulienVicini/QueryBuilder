using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.Statements;

namespace QueryBuilder.Core.Services
{
    public class BulkMergeService<TRecord>
        : IBulkMergeService<TRecord>
        where TRecord : class
    {
        private const string TEMP_TABLE = "#tmp_bulk";
        private readonly IBulkInsertService<TRecord> _bulkInsertService;
        private readonly ICommandService<TRecord> _commandService;

        public BulkMergeService(IBulkInsertService<TRecord> bulkInsertService, ICommandService<TRecord> commandService)
        {
            _bulkInsertService = bulkInsertService ?? throw new ArgumentNullException(nameof(bulkInsertService));
            _commandService    = commandService    ?? throw new ArgumentNullException(nameof(commandService));
        }

        public void WriteToServer(IEnumerable<TRecord> records, IEnumerable<MemberExpression> mergeKeys, bool updateOnly)
        {
            Check.NotNullOrEmpty(records, nameof(records));
            Check.NotNullOrEmpty(mergeKeys, nameof(mergeKeys));

            // Create Temp table
            DropCreateTemporaryTable(AlterTableStatement<TRecord>.AlterType.Create);

            // Insert Data
            _bulkInsertService.WriteToServer(records, TEMP_TABLE);

            // Merge Query
            MergeTables(mergeKeys, updateOnly);

            // Drop Table
            DropCreateTemporaryTable(AlterTableStatement<TRecord>.AlterType.Drop);
        }

        public void DropCreateTemporaryTable(AlterTableStatement<TRecord>.AlterType alterType)
        {
            _commandService.Alter(
                new AlterTableStatement<TRecord>(TEMP_TABLE, alterType)
            );
        }

        public void MergeTables(IEnumerable<MemberExpression> mergeKeys, bool isUpdateOnly)
        {
            MergeStatement<TRecord>.MergeType mergeType = GetMergeType(isUpdateOnly);

            var mergeQuery = new MergeStatement<TRecord>(mergeKeys, TEMP_TABLE, mergeType);

            _commandService.Merge(mergeQuery);
        }

        public MergeStatement<TRecord>.MergeType GetMergeType(bool isUpdateOnly)
        {
            return isUpdateOnly ? MergeStatement<TRecord>.MergeType.UpdateOnly
                                : MergeStatement<TRecord>.MergeType.InsertOrUpdate;
        }
    }
}
