using EntityFramework.Extensions.Core.Queries.Statements.Operators;
using EntityFramework.Extensions.Core.Queries.Statements.Values;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Queries.Statements
{
    public interface IStatementTranslator
    {
        (string query, IEnumerable<object> parameters) Translate(IVisitableStatement statement);

        #region Values

        void TranslateConstantStatement<TConstant>(ConstantStatement<TConstant> valueStatement);

        void TranslateColumnExpressionStatement<TEntity>(ColumnValueStatement<TEntity> columnExpressionStatement) where TEntity : class;

        #endregion

        #region Operators

        void TranslateArithmeticStatement(ArithmeticStatement arithmeticStatement);

        void TranslateAssignStatement<TEntity>(AssignStatement<TEntity> assignStatement) where TEntity : class;

        void TranslateComparisonStatement(ComparisonStatement comparisonStatement);

        void TranslateConditionalStatement(ConditionalStatement conditionalStatement);

        void TranslateRangeStatement<TValue>(RangeConstantStatement<TValue> rangeStatement);

        #endregion
    }
}
