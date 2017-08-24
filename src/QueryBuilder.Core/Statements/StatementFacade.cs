using QueryBuilder.Core.Database;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Core.Statements
{
    public class StatementFacade<TEntity>
        where TEntity : class
    {
        private readonly IStatementTranslator<TEntity> _queryTranslator;

        private readonly ICommandProcessing _queryProcessor;

        public StatementFacade(IStatementTranslator<TEntity> queryTranslator, ICommandProcessing queryProcessor)
        {
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
            _queryProcessor  = queryProcessor  ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        public int Alter(AlterTableStatement<TEntity> alterQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(alterQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Update(UpdateStatement<TEntity> updateQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(updateQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Delete(DeleteStatement<TEntity> deleteQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(deleteQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Merge(MergeStatement<TEntity> mergeQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(mergeQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }
    }
}
