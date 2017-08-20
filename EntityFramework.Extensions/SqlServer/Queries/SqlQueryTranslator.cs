using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public class SqlQueryTranslator<TEntity>
        : IQueryTranslator<TEntity>
        where TEntity : class
    {
        private readonly IMappingAdapter<TEntity> _mappingAdapter;
        private readonly SQLStatementGenerator<TEntity> _statementGeneration;

        public SqlQueryTranslator(IMappingAdapter<TEntity> mappingAdapter)
        {
            _mappingAdapter = mappingAdapter;
            _statementGeneration = new SQLStatementGenerator<TEntity>(mappingAdapter);
        }

        public (string, IEnumerable<object>) TranslateQuery(DeleteQuery<TEntity> query)
        {
            StringBuilder stringBuilder = new StringBuilder();

            AppendOperation(stringBuilder, "DELETE");

            SqlParameterCollection queryParameters = null;

            if (query.Predicate != null)
                queryParameters = AppendPredicate( stringBuilder, query.Predicate );

            return (
                stringBuilder.ToString(),
                queryParameters?.Parameters.ToArray()
            );
        }

        protected void AppendOperation(StringBuilder builder, string operation)
        {
            builder.Append(operation)
                   .Append(" ")
                   .Append(_mappingAdapter.GetTableName());
        }

        protected SqlParameterCollection AppendPredicate( StringBuilder stringBuilder, Expression<Func<TEntity, bool>> predicate )
        {
            (string query, SqlParameterCollection parameters) = _statementGeneration.Generate(predicate);

            stringBuilder.Append(" WHERE ")
                         .Append(query);

            return parameters;
        }
    }
}
