using EntityFramework.Extensions.Core.Queries.Statements;
using EntityFramework.Extensions.Core.Queries.Statements.Operators;
using EntityFramework.Extensions.Core.Queries.Statements.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public class SQLStatementTranslator
        : IStatementTranslator
    {
        private SqlParameterCollection _sqlParameterCollection;
        private StringBuilder _queryBuilder;

        public SQLStatementTranslator() { }

        public (string query, IEnumerable<object> parameters) Translate(IVisitableStatement statement)
        {
            _sqlParameterCollection = new SqlParameterCollection();
            _queryBuilder           = new StringBuilder();

            statement.Visit(this);

            return (
                query     : _queryBuilder.ToString(),
                parameters: _sqlParameterCollection.Parameters
            );
        }

        #region Values

        public void TranslateConstantStatement<TConstant>(ConstantStatement<TConstant> valueStatement)
        {
            _queryBuilder.Append(
                _sqlParameterCollection.AddParameter(valueStatement.Value)
            );
        }

        public void TranslateColumnExpressionStatement<TEntity>(ColumnValueStatement<TEntity> columnExpressionStatement) where TEntity : class
        {
            _queryBuilder.Append(
                columnExpressionStatement.ColumnMapping.DbColumnName
            );
        }

        #endregion

        #region Operators

        public void TranslateArithmeticStatement(ArithmeticStatement arithmeticStatement)
        {
            string sqlOperator = SQLOperatorConverter.Convert(arithmeticStatement.Operator);

            arithmeticStatement.Left.Visit(this);
            _queryBuilder.Append( " " + sqlOperator + " ");
            arithmeticStatement.Right.Visit(this);
        }

        public void TranslateAssignStatement<TEntity>(AssignStatement<TEntity> assignStatement) where TEntity : class
        {
            assignStatement.Column.Visit(this);
            _queryBuilder.Append(" = ");
            assignStatement.Value.Visit(this);
        }

        public void TranslateComparisonStatement(ComparisonStatement comparisonStatement)
        {
            string sqlOperator = SQLOperatorConverter.Convert(comparisonStatement.Operator);

            comparisonStatement.Left.Visit(this);
            _queryBuilder.Append(" " + sqlOperator + " ");
            comparisonStatement.Right.Visit(this);
        }

        public void TranslateConditionalStatement(ConditionalStatement conditionalStatement)
        {
            string sqlOperator = SQLOperatorConverter.Convert(conditionalStatement.Operator);

            _queryBuilder.Append("(");

            conditionalStatement.Left.Visit(this);
            _queryBuilder.Append(" " + sqlOperator + " ");
            conditionalStatement.Right.Visit(this);

            _queryBuilder.Append(")");
        }

        public void TranslateRangeStatement<TValue>(RangeConstantStatement<TValue> rangeStatement)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
