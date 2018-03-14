using QueryBuilder.Core.Database;
using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.Statements;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Core.Services
{
    public class CommandService<TEntity, TTransaction>
        : ICommandService<TEntity>,
        ITransactionalCommandService<TEntity, TTransaction>
        where TEntity : class
    {
        private readonly IStatementTranslator<TEntity> _queryTranslator;
        private readonly ICommandProcessing<TTransaction> _queryProcessor;

        public CommandService(IStatementTranslator<TEntity> queryTranslator, ICommandProcessing<TTransaction> queryProcessor)
        {
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
            _queryProcessor  = queryProcessor  ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        public int Alter(AlterTableStatement<TEntity> alterQuery)
        {
            Check.NotNull(alterQuery, nameof(alterQuery));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(alterQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Alter(AlterTableStatement<TEntity> alterQuery, TTransaction transaction)
        {
            Check.NotNull(alterQuery, nameof(alterQuery));
            Check.NotNull(transaction, nameof(transaction));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(alterQuery);

            return _queryProcessor.ExecuteCommand(
                query      : query,
                parameters : parameters,
                transaction: transaction
            );
        }

        public int Update(UpdateStatement<TEntity> updateQuery)
        {
            Check.NotNull(updateQuery, nameof(updateQuery));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(updateQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Update(UpdateStatement<TEntity> updateQuery, TTransaction transaction)
        {
            Check.NotNull(updateQuery, nameof(updateQuery));
            Check.NotNull(transaction, nameof(transaction));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(updateQuery);

            return _queryProcessor.ExecuteCommand(
                query      : query,
                parameters : parameters,
                transaction: transaction
            );
        }

        public int Delete(DeleteStatement<TEntity> deleteQuery)
        {
            Check.NotNull(deleteQuery, nameof(deleteQuery));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(deleteQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Delete(DeleteStatement<TEntity> deleteQuery, TTransaction transaction)
        {
            Check.NotNull(deleteQuery, nameof(deleteQuery));
            Check.NotNull(transaction, nameof(transaction));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(deleteQuery);

            return _queryProcessor.ExecuteCommand(
                query      : query,
                parameters : parameters,
                transaction: transaction
            );
        }

        public int Merge(MergeStatement<TEntity> mergeQuery)
        {
            Check.NotNull(mergeQuery, nameof(mergeQuery));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(mergeQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Merge(MergeStatement<TEntity> mergeQuery, TTransaction transaction)
        {
            Check.NotNull(mergeQuery, nameof(mergeQuery));
            Check.NotNull(transaction, nameof(transaction));

            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(mergeQuery);

            return _queryProcessor.ExecuteCommand(
                query      : query,
                parameters : parameters,
                transaction: transaction
            );
        }
    }
}
