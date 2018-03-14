using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Tests.FakeModels;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Xunit;

namespace QueryBuilder.Core.Tests.Mappings
{
    public class ColumnMappingTests
    {
        #region

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

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenColumnNameIsNull()
        {
            Assert.Throws<ArgumentException>(() =>
                CreateTestClassInstance(
                    dbColumnName: null,
                    property    : typeof(TestClass).GetProperties().First()
                )
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenColumnNameIsWhiteSpace()
        {
            Assert.Throws<ArgumentException>(() =>
                CreateTestClassInstance(
                    dbColumnName: "    ",
                    property    : typeof(TestClass).GetProperties().First()
                )
            );
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenPropertyInfoIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                CreateTestClassInstance(
                    dbColumnName: "MyDbColumnName",
                    property    : null
                )
            );
        }

        [Fact]
        public void ConstructorThrowsInvalidOperationExceptionWhenPropertyInfoIsNotAPropertyOfTEntity()
        {
            Assert.Throws<InvalidOperationException>(() =>
                CreateTestClassInstance(
                    dbColumnName: "MyDbColumnName",
                    property    : typeof(OtherClass).GetProperties().First()
                )
            );
        }

        [Fact]
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
            Assert.Equal(columnName              , mapping.DbColumnName );
            Assert.Equal(DbType.StringFixedLength, mapping.DbType       );
            Assert.Equal(10                      , mapping.Length       );
            Assert.Equal(11                      , mapping.Precision    );
            Assert.Equal(12                      , mapping.Scale        );
            Assert.Equal(property                , mapping.PropertyInfo );
            Assert.True(mapping.IsIdentity);
            Assert.True(mapping.IsRequired);
        }
    }
}
