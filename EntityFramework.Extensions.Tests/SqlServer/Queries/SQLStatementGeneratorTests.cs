using EntityFramework.Extensions.Core.Mappings;
using EntityFramework.Extensions.SqlServer.Queries;
using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.Tests.SqlServer.Queries
{
    [TestClass]
    public class SQLStatementGeneratorTests
    {
        private SQLStatementGenerator<Parent> _generator;
        private string _query;
        private IEnumerable<SqlParameter> _parameters;

        [TestInitialize]
        public void Init()
        {
            var mappingAdapterMock = new Mock<IMappingAdapter<Parent>>();

            mappingAdapterMock.Setup(m => m.GetColumns())
                              .Returns(new List<ColumnMapping<Parent>>()
                              {
                                  new ColumnMapping<Parent>("Id"     , true , typeof(Parent).GetProperty(nameof(Parent.Id           ))),
                                  new ColumnMapping<Parent>("P_First", false, typeof(Parent).GetProperty(nameof(Parent.FirstVariable)))
                              });

            _generator = new SQLStatementGenerator<Parent>( mappingAdapterMock.Object );
            _query = null;
            _parameters = null;
        }

        [TestMethod]
        public void GenerateReturnsConstante()
        {
            var constanteExpr = Expression.Constant(3);

            (_query, _parameters) = _generator.Generate(constanteExpr);
            Assert.AreEqual(_query, "@p0");
            Assert.AreEqual(1, _parameters.Count()      );
            Assert.AreEqual(3, _parameters.First().Value);
        }

        [TestMethod]
        public void GenerateReturnsAddition()
        {
            var additionExpr = Expression.Add(
                Expression.Constant(3),
                Expression.Constant(2)
            );

            (_query, _parameters)  = _generator.Generate(additionExpr);
            Assert.AreEqual(_query, "(@p0 + @p1)");
            Assert.AreEqual(2, _parameters.Count());

            SqlParameter leftParameter  = _parameters.First();
            SqlParameter rightParameter = _parameters.Last();

            Assert.AreEqual(3    , leftParameter.Value);
            Assert.AreEqual("@p0", leftParameter.ParameterName);

            Assert.AreEqual(2    , rightParameter.Value);
            Assert.AreEqual("@p1", rightParameter.ParameterName);
        }

        [TestMethod]
        public void GenerateReturnsMemberExpression()
        {
            Expression<Func<Parent, string>> memberExpression = p => p.FirstVariable;

            (_query, _parameters) = _generator.Generate(memberExpression);

            Assert.AreEqual(0, _parameters.Count());
            Assert.AreEqual("P_First", _query);
        }

        [TestMethod]
        public void GenerateReturnsOrExpression()
        {
            Expression<Func<Parent, bool>> predicate = p => p.Id == 3 || p.Id == 4;

            (_query, _parameters) = _generator.Generate(predicate);

            Assert.AreEqual(2, _parameters.Count());

            SqlParameter firstParameter  = _parameters.First();
            SqlParameter secondParameter = _parameters.Last();

            Assert.AreEqual("@p0", firstParameter.ParameterName);
            Assert.AreEqual("@p1", secondParameter.ParameterName);
            Assert.AreEqual(3, firstParameter.Value);
            Assert.AreEqual(4, secondParameter.Value);

            Assert.AreEqual("((Id = @p0) OR (Id = @p1))", _query);
        }
    }
}
