using EntityFramework.Extensions.Core.Mappings;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public class SQLStatementGenerator<TEntity> 
        : ExpressionVisitor
        where TEntity : class
    {
        private SqlParameterCollection _parameters;

        private StringBuilder _stringBuilder;

        private IMappingAdapter<TEntity> _mappingAdapter;

        public SQLStatementGenerator(IMappingAdapter<TEntity> mappingAdapter)
        {
            _mappingAdapter = mappingAdapter;
        }

        private void Init()
        {
            _parameters = new SqlParameterCollection();
            _stringBuilder = new StringBuilder();
        }

        public (string query, SqlParameterCollection paramters) Generate( Expression expression )
        {
            Init();

            Visit(expression);

            return (_stringBuilder.ToString(), _parameters);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            string memberName = node.Member.Name;

            ColumnMapping<TEntity> column
                = _mappingAdapter.GetColumns()
                                 .First(c => c.PropertyInfo.Name == memberName);

            _stringBuilder.Append(column.DbColumnName);

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _stringBuilder.Append("(");

            base.Visit(node.Left);

            _stringBuilder.Append(" ")
                          .Append(ExpressionSQLTranslatorHelpers.GetOpertor(node))
                          .Append(" ");

            base.Visit(node.Right);

            _stringBuilder.Append(")");

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _stringBuilder.Append(
                _parameters.AddParameter(node.Value)
            );

            return base.VisitConstant(node);
        }
    }
}
