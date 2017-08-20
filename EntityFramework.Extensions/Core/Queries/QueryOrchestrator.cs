using EntityFramework.Extensions.Core.Database;
using System;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Queries
{
    public class QueryOrchestrator<TEntity>
        where TEntity : class
    {
        private readonly IQueryTranslator<TEntity> _queryTranslator;

        private readonly ICommandProcessing _queryProcessor;

        public QueryOrchestrator(IQueryTranslator<TEntity> queryTranslator, ICommandProcessing queryProcessor)
        {
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
            _queryProcessor  = queryProcessor  ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        public int Update(UpdateQuery<TEntity> updateQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(updateQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }

        public int Delete(DeleteQuery<TEntity> deleteQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(deleteQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }
    }
}
