﻿using QueryBuilder.Core.Database;
using System;
using System.Collections.Generic;

namespace QueryBuilder.Core.Queries
{
    public class QueryCoordinator<TEntity>
        where TEntity : class
    {
        private readonly IQueryTranslator<TEntity> _queryTranslator;

        private readonly ICommandProcessing _queryProcessor;

        public QueryCoordinator(IQueryTranslator<TEntity> queryTranslator, ICommandProcessing queryProcessor)
        {
            _queryTranslator = queryTranslator ?? throw new ArgumentNullException(nameof(queryTranslator));
            _queryProcessor  = queryProcessor  ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        public int Alter(AlterTableQuery<TEntity> alterQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(alterQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
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

        public int Merge(MergeQuery<TEntity> mergeQuery)
        {
            (string query, IEnumerable<object> parameters) = _queryTranslator.TranslateQuery(mergeQuery);

            return _queryProcessor.ExecuteCommand(
                query     : query,
                parameters: parameters
            );
        }
    }
}