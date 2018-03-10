using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryBuilder.Core.Mappings;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace QueryBuilder.Core.Tests.Mappings
{
    [TestClass]
    public class ColumnMappingTests
    {
        #region

        private class TestClass
        {
            public int Id { get; set; }
        }

        private class OtherClass
        {
            public int Id { get; set; }
        }

        private ColumnMapping<TestClass> CreateTestClassInstance(string dbColumnName, PropertyInfo property)
        {
            return new ColumnMapping<TestClass>(
                dbColumnName: dbColumnName,
                dbType      : DbType.StringFixedLength,
                isIdentity  : true,
                isRequired  : true,
                length      : 10,
                precision   : 11,
                propertyInfo: property,
                scale       : 12
            );
        }

        #endregion

        public void ConstructorThrowsArgumentExceptionWhenColumnNameIsNull()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                CreateTestClassInstance(
                    dbColumnName: null,
                    property    : typeof(TestClass).GetProperties().First()
                )
            );
        }

        public void ConstructorThrowsArgumentExceptionWhenColumnNameIsWhiteSpace()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                CreateTestClassInstance(
                    dbColumnName: "    ",
                    property    : typeof(TestClass).GetProperties().First()
                )
            );
        }

        public void ConstructorThrowsArgumentNullExceptionWhenPropertyInfoIsNull()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                CreateTestClassInstance(
                    dbColumnName: "MyDbColumnName",
                    property    : null
                )
            );
        }

        public void ConstructorThrowsInvalidOperationExceptionWhenPropertyInfoIsNotAPropertyOfTEntity()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                CreateTestClassInstance(
                    dbColumnName: "MyDbColumnName",
                    property    : typeof(OtherClass).GetProperties().First()
                )
            );
        }

        [TestMethod]
        public void ConstructorShouldSetProperties()
        {
            const string columnName = "ABCDE";
            PropertyInfo property = typeof(TestClass).GetProperties()
                                                     .First();

            ColumnMapping<TestClass> mapping = CreateTestClassInstance(
                dbColumnName: columnName,
                property    : property
            );

            // Assert
            Assert.AreEqual(columnName              , mapping.DbColumnName );
            Assert.AreEqual(DbType.StringFixedLength, mapping.DbType       );
            Assert.AreEqual(true                    , mapping.IsIdentity   );
            Assert.AreEqual(true                    , mapping.IsRequired   );
            Assert.AreEqual(10                      , mapping.Length       );
            Assert.AreEqual(11                      , mapping.Precision    );
            Assert.AreEqual(12                      , mapping.Scale        );
            Assert.AreEqual(property                , mapping.PropertyInfo );
        }
    }
}
