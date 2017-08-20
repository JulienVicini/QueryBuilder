using EntityFramework.Extensions.Core.Mappings;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public class SQLStatementGenerator<TEntity> 
        : ExpressionVisitor
        where TEntity : class
    {
        private SqlQueryBuilder _queryBuilder;

        private IMappingAdapter<TEntity> _mappingAdapter;

        public SQLStatementGenerator(IMappingAdapter<TEntity> mappingAdapter)
        {
            _mappingAdapter = mappingAdapter;
        }

        public SqlQueryBuilder Generate( Expression expression )
        {
            _queryBuilder = new SqlQueryBuilder();

            AppendPart(_queryBuilder, expression);

            return _queryBuilder;
        } 

        public void AppendPart( SqlQueryBuilder queryBuilder, Expression expression )
        {
            _queryBuilder = queryBuilder;

            Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            string memberName = node.Member.Name;

            ColumnMapping<TEntity> column
                = _mappingAdapter.GetColumns()
                                 .First(c => c.PropertyInfo.Name == memberName);

            _queryBuilder.Query.Append(column.DbColumnName);

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            //_queryBuilder.Query.Append("(");

            base.Visit(node.Left);

            _queryBuilder.Query.Append(" ")
                               .Append(ExpressionSQLTranslatorHelpers.GetOpertor(node))
                               .Append(" ");

            base.Visit(node.Right);

            //_queryBuilder.Query.Append(")");

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _queryBuilder.AppendValue(node.Value);

            return base.VisitConstant(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.VisitUnary(node);
        }
    }
}
