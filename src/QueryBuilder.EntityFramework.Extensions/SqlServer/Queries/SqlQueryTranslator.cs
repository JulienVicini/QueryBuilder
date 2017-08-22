using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Queries;
using QueryBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.Extensions.SqlServer.Queries
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

        #region Create / Drop Table

        public (string, IEnumerable<object>) TranslateQuery(AlterTableQuery<TEntity> query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            switch (query.Type)
            {
                case AlterTableQuery<TEntity>.AlterType.Create: return GenerateCreateQuery(query.TableName).Build();
                case AlterTableQuery<TEntity>.AlterType.Drop  : return GenerateDropQuery(query.TableName).Build();
                default:
                    throw new InvalidOperationException($"The alter operation\"{query.Type}\" is not supported.");
            }
        }

        public SqlQueryBuilder GenerateDropQuery(string tableName)
        {
            var queryBuilder = new SqlQueryBuilder();

            AppendOperation(queryBuilder, "DROP TABLE", tableName);

            return queryBuilder;
        }

        public SqlQueryBuilder GenerateCreateQuery(string tableName)
        {
            var queryBuilder = new SqlQueryBuilder();

            AppendOperation(queryBuilder, "CREATE TABLE", tableName);
            queryBuilder.Append(" (");

            GenerateColumnDefinition(queryBuilder, _mappingAdapter.GetColumns().Where(c => !c.IsIdentity));

            queryBuilder.Append(" )");

            return queryBuilder;
        }

        public void GenerateColumnDefinition(SqlQueryBuilder queryBuilder, IEnumerable<ColumnMapping<TEntity>> mappings)
        {
            bool firstColumn = true;

            foreach(ColumnMapping<TEntity> column in mappings)
            {
                if (firstColumn) firstColumn = false;
                else queryBuilder.Append(", ");

                queryBuilder.Append(column.DbColumnName)
                            .Append(" ")
                            .Append(SqlTypeTranslator.TranslateType<TEntity>(column));

                if (column.IsRequired)
                    queryBuilder.Append(" NOT NULL");
                else
                    queryBuilder.Append(" NULL");
            }
        }

        #endregion

        #region MergeQuery

        const string SOURCE_TABLE_ALIAS = "Source";
        const string TARGET_TABLE_ALIAS = "Target";

        public (string, IEnumerable<object>) TranslateQuery(MergeQuery<TEntity> mergeQuery)
        {
            var queryBuilder = new SqlQueryBuilder();

            // Merge Statement
            AppendOperation(queryBuilder, "MERGE INTO", tableAlias: TARGET_TABLE_ALIAS);

            // Using Statement
            AppendOperation(queryBuilder, " USING", mergeQuery.TemporaryTableName, SOURCE_TABLE_ALIAS);
            queryBuilder.Append(" ON ");
            AppendMergeJoinKeys(queryBuilder, mergeQuery.Keys);

            // Update Into
            if( mergeQuery.Type == MergeQuery<TEntity>.MergeType.UpdateOnly || mergeQuery.Type == MergeQuery<TEntity>.MergeType.InsertOrUpdate )
                AppendMergeStatement(queryBuilder);

            // Insert Statement
            if (mergeQuery.Type == MergeQuery<TEntity>.MergeType.InsertOnly || mergeQuery.Type == MergeQuery<TEntity>.MergeType.InsertOrUpdate)
                AppendInsertMergeStatement(queryBuilder);

            queryBuilder.Append(";");

            return queryBuilder.Build();
        }

        public void AppendMergeJoinKeys(SqlQueryBuilder queryBuilder, IEnumerable<MemberExpression> expressions)
        {
            bool first = true;

            foreach(MemberExpression expression in expressions)
            {
                if (first) first = false;
                else queryBuilder.Append(" AND ");

                AppendSourceAndTargetOperator(queryBuilder, expression, ExpressionType.Equal);
            }
        }

        public void AppendSourceAndTargetOperator(SqlQueryBuilder queryBuilder, MemberExpression member, ExpressionType @operator)
        {
            // Source
            AppendAliasMember(queryBuilder, SOURCE_TABLE_ALIAS, member);

            // Operator
            queryBuilder.Append(" ")
                        .Append(ExpressionSQLTranslatorHelpers.GetOpertor(@operator))
                        .Append(" ");

            // Destination
            AppendAliasMember(queryBuilder, TARGET_TABLE_ALIAS, member);
        }

        public void AppendAliasMember(SqlQueryBuilder queryBuilder, string tableAlias, MemberExpression member)
        {
            // Source 
            queryBuilder.Append(tableAlias)
                        .Append(".[");
            _statementGeneration.AppendPart(queryBuilder, member);
            queryBuilder.Append("]");
        }

        public void AppendMergeStatement(SqlQueryBuilder queryBuilder)
        {
            queryBuilder.Append(" WHEN MATCHED THEN UPDATE SET ");

            bool isFirst = true;
            foreach (ColumnMapping<TEntity> column in _mappingAdapter.GetColumns().Where(c => !c.IsIdentity))
            {
                if (isFirst) isFirst = false;
                else queryBuilder.Append(", ");

                queryBuilder.Append(TARGET_TABLE_ALIAS)
                            .Append(".[")
                            .Append(column.DbColumnName)
                            .Append("] = ")
                            .Append(SOURCE_TABLE_ALIAS)
                            .Append(".[")
                            .Append(column.DbColumnName)
                            .Append("]");
            }
        }

        public void AppendInsertMergeStatement(SqlQueryBuilder queryBuilder)
        {
            List<ColumnMapping<TEntity>> columns = _mappingAdapter.GetColumns()
                                                                  .Where(c => !c.IsIdentity)
                                                                  .ToList();

            queryBuilder.Append(" WHEN NOT MATCHED THEN INSERT (")
                        .Append(
                            string.Join(", ", columns.Select(c => c.DbColumnName))
                        )
                        .Append(") VALUES (")
                        .Append(
                            string.Join(", ", columns.Select(c => SOURCE_TABLE_ALIAS + ".["+ c.DbColumnName+"]"))
                        )
                        .Append(")");
        }

        #endregion

        #region Commons

        protected void AppendOperation(SqlQueryBuilder builder, string operation, string tableName = null, string tableAlias = null)
        {
            builder.Append(operation)
                   .Append(" ")
                   .Append(tableName ?? _mappingAdapter.GetTableName());

            if (tableAlias != null)
                builder.Append(" AS ")
                       .Append(tableAlias);
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

                Expression asssignExpression = ExpressionHelper.MakeAssign(assignment);

                _statementGeneration.AppendPart(queryBuilder, asssignExpression);
            }
        }

        #endregion
    }
}
