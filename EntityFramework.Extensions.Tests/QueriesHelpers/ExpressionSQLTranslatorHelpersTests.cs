using EntityFramework.Extensions.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Tests.QueriesHelpers
{
    [TestClass]
    public class ExpressionSQLTranslatorHelpersTests
    {
        #region Test Helpers

        private void AssertOperator<T>(string expectedOperator, Expression<Func<T, bool>> predicate)
            => AssertTranslation(expectedOperator, predicate);

        private void AssertOperator<T>(string expectedOperator, Expression<Func<T, T>> operation)
            => AssertTranslation(expectedOperator, operation);

        private void AssertTranslation<T, TResult>(string expectedOperator, Expression<Func<T, TResult>> predicate)
        {
            BinaryExpression binaryExpr = predicate.Body as BinaryExpression;

            Assert.IsNotNull(binaryExpr);

            Expression<Func<int, int>> func = (i) => i + 2;

            Assert.AreEqual(
                expectedOperator,
                ExpressionSQLTranslatorHelpers.GetOpertor(binaryExpr.NodeType)
            );
        }

        #endregion

        #region Comparison/Logical Operators

        [TestMethod]
        public void GetLogicalOpertorReturnsEquals() => AssertOperator<int>("=", _ => _ == 2);

        [TestMethod]
        public void GetLogicalOpertorReturnsNotEquals() => AssertOperator<int>("<>", _ => _ != 2);

        [TestMethod]
        public void GetLogicalOperatorReturnsGreatherThan() => AssertOperator<int>(">", _ => _ > 10);

        [TestMethod]
        public void GetLogicalOperatorReturnsGreatherThanOrEquals() => AssertOperator<int>(">=", _ => _ >= 10);

        [TestMethod]
        public void GetLogicalOperatorReturnsLowerThan() => AssertOperator<int>("<", _ => _ < 10);

        [TestMethod]
        public void GetLogicalOperatorReturnsLowerThanOrEquals() => AssertOperator<int>("<=", _ => _ <= 10);

        [TestMethod]
        public void GetLogicalOpertorReturnsAnd() => AssertOperator<bool>("AND", _ => _ && true);

        [TestMethod]
        public void GetLogicalOpertorReturnsOr() => AssertOperator<bool>("OR", _ => _ || true);

        #endregion

        #region Arithmetic Operators

        [TestMethod]
        public void GetArithmeticOperatorReturnsAdd() => AssertOperator<int>("+", _ => _ + 1);

        [TestMethod]
        public void GetArithmeticOperatorReturnsSubstract() => AssertOperator<int>("-", _ => _ - 1);

        [TestMethod]
        public void GetArithmeticOperatorReturnsDivide() => AssertOperator<int>("/", _ => _ / 1);

        [TestMethod]
        public void GetArithmeticOperatorReturnsMultiply() => AssertOperator<int>("*", _ => _ * 1);

        [TestMethod]
        public void GetArithmeticOperatorReturnsModulo() => AssertOperator<int>("%", _ => _ % 1);

        #endregion

        #region Functions translation

        // Contains
        // Like

        #endregion
    }
}
