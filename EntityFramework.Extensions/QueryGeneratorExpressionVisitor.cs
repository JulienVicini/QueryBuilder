using EntityFramework.Extensions.SqlServer;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EntityFramework.Extensions
{
    public class QueryGeneratorExpressionVisitor : ExpressionVisitor
    {
        private readonly StringBuilder _queryBuilder;
        private readonly SqlParameterCollection _sqlParameterCollection;

        public int MyProperty { get; private set; }

        public QueryGeneratorExpressionVisitor()
        {
            _queryBuilder = new StringBuilder();
            _sqlParameterCollection = new SqlParameterCollection();
        }

        public string GetSQLPredicate()
        {
            return _queryBuilder.ToString();
        }

        public IEnumerable<System.Data.SqlClient.SqlParameter> GetSqlParameter()
        {
            return _sqlParameterCollection.Parameters;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            string parameterName = _sqlParameterCollection.AddParameter( node.Value );
            _queryBuilder.Append(parameterName);
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _queryBuilder.Append(node.Member.Name);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            // LEFT PART
            _queryBuilder.Append("(");
            base.Visit(node.Left);
            _queryBuilder.Append(")");

            // Operator
            _queryBuilder.Append(" " + ExpressionSQLTranslatorHelpers.GetOpertor(node) + " ");

            // RIGHT PART
            _queryBuilder.Append("(");
            base.Visit(node.Right);
            _queryBuilder.Append(")");

            return node;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return base.VisitConditional(node);
        }

    }
}
