using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Tests
{
    [TestClass]
    public class QueryGeneratorVisitorTests
    {
        [TestMethod]
        public void Visit_Ok()
        {
            bool val = true && true || false;
            bool val2 = true || false && true;
            bool val3 = false && false || true;

            var visitor = new QueryGeneratorExpressionVisitor();

            Expression<Func<Parent, bool>> predicate
                = p => p.Id > 4 && (p.FirstVariable == "YES" || p.SecondVariable != 0d);

            visitor.Visit(predicate);

            string query = visitor.GetSQLPredicate();
            var parameters = visitor.GetSqlParameter();
        }
    }
}
