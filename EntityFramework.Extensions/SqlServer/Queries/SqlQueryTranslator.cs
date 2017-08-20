using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.Core.Queries;
using EntityFramework.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        #region Delete

        public (string, IEnumerable<object>) TranslateQuery(DeleteQuery<TEntity> query)
        {
            var queryBuilder = new SqlQueryBuilder();

            AppendOperation(queryBuilder, "DELETE");
            AppendPredicate( queryBuilder, query.Predicate );

            return queryBuilder.Build();
        }

        #endregion

        #region Update

        public (string, IEnumerable<object>) TranslateQuery(UpdateQuery<TEntity> query)
        {
            var queryBuilder = new SqlQueryBuilder();

            AppendOperation(queryBuilder, "UPDATE");
            AppendUpdateStatements(queryBuilder, query.Assignments);
            AppendPredicate(queryBuilder, query.Predicate);

            return queryBuilder.Build();
        }

        #endregion

        #region Commons

        protected void AppendOperation(SqlQueryBuilder builder, string operation)
        {
            builder.Append(operation)
                   .Append(" ")
                   .Append(_mappingAdapter.GetTableName());
        }

        protected void AppendPredicate( SqlQueryBuilder queryBuilder, Expression<Func<TEntity, bool>> predicate )
        {
            if (predicate == null)
                return;

            queryBuilder.Append(" WHERE ");

            _statementGeneration.AppendPart(queryBuilder, predicate);
        }

        protected void AppendUpdateStatements( SqlQueryBuilder queryBuilder, IEnumerable<MemberAssignment> assignments )
        {
            queryBuilder.Append(" SET ");

            bool first = true;
            
            foreach(MemberAssignment assignment in assignments)
            {
                if (first) first = false;
                else queryBuilder.Append(", ");

                Expression asssignExpression = ExpressionHelpers.MakeAssign(assignment);

                _statementGeneration.AppendPart(queryBuilder, asssignExpression);
            }
        }

        #endregion
    }
}
