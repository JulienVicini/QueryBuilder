using System;
using System.Data;
using QueryBuilder.EFCore.Mappings;
using Xunit;

namespace QueryBuilder.EFCore.Tests.Mappings
{
    public class RelationalSqlTypePropertyTranslatorTests
    {
        private readonly RelationalSqlTypePropertyTranslator _typeReader;

        public RelationalSqlTypePropertyTranslatorTests()
        {
            _typeReader = new RelationalSqlTypePropertyTranslator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void GetPrecisionAndScaleThrowsArgumentExceptionWhenSqlTypeIsNullOrWhiteSpace(string sqlType)
        {
            Assert.Throws<ArgumentException>(
                () => _typeReader.GetMetaData(sqlType, out int? precision, out int? scale, out DbType expectedDbType)
            );
        }

        [Theory]
        [InlineData("Decimal(18,8)"       , DbType.Decimal)]
        [InlineData("decimal( 18, 8 )"    , DbType.Decimal)]
        [InlineData("decimal(18, 8)"      , DbType.Decimal)]
        [InlineData("   numeric(18, 8)   ", DbType.Decimal)]
        [InlineData("numeric( 18,8 )"     , DbType.Decimal)]
        public void GetPrecisionAndScaleReturnsPrecisionAndScaleWhenSqlTypeIsDecimalOrNumeric(string sqlType, DbType expectedDbType)
        {
            // Act
            _typeReader.GetMetaData(sqlType, out int? precision, out int? scale, out DbType actualDbType);

            // Assert
            Assert.Equal(expectedDbType, actualDbType);

            Assert.NotNull(precision);
            Assert.Equal(18, precision.Value);

            Assert.NotNull(scale);
            Assert.Equal(8, scale.Value);
        }

        [Theory]
        [InlineData("nvarchar(20)", DbType.String)]
        [InlineData("date", DbType.Date)]
        [InlineData("binary(16)", DbType.Binary)]
        [InlineData("int", DbType.Int32)]
        [InlineData("money", DbType.Decimal)]
        public void GetPrecisionAndScaleReturnsNullWhenSqlTypeIsNotDecimalOrNumeric(string sqlType, DbType expectedDbType)
        {
            // Act
            _typeReader.GetMetaData(sqlType, out int? precision, out int? scale, out DbType actualDbType);

            // Assert
            Assert.Equal(expectedDbType, actualDbType);
            Assert.Null(precision);
            Assert.Null(scale);
        }

        [Theory]
        [InlineData("bigint", DbType.Int64)]
        [InlineData("binary", DbType.Binary)]
        [InlineData("bit", DbType.Boolean)]
        [InlineData("char", DbType.AnsiStringFixedLength)]
        [InlineData("date", DbType.Date)]
        [InlineData("datetime", DbType.DateTime)]
        [InlineData("datetime2", DbType.DateTime2)]
        [InlineData("datetimeoffset", DbType.DateTimeOffset)]
        [InlineData("decimal", DbType.Decimal)]
        [InlineData("float", DbType.Double)]
        [InlineData("image", DbType.Binary)]
        [InlineData("int", DbType.Int32)]
        [InlineData("money", DbType.Decimal)]
        [InlineData("nchar", DbType.StringFixedLength)]
        [InlineData("ntext", DbType.String)]
        [InlineData("numeric", DbType.Decimal)]
        [InlineData("nvarchar", DbType.String)]
        [InlineData("real", DbType.Single)]
        [InlineData("rowversion", DbType.Binary)]
        [InlineData("smalldatetime", DbType.DateTime)]
        [InlineData("smallint", DbType.Int16)]
        [InlineData("smallmoney", DbType.Decimal)]
        [InlineData("sql_variant", DbType.Object)]
        [InlineData("text", DbType.String)]
        [InlineData("time", DbType.Time)]
        [InlineData("timestamp", DbType.Binary)]
        [InlineData("tinyint", DbType.Byte)]
        [InlineData("uniqueidentifier", DbType.Guid)]
        [InlineData("varbinary", DbType.Binary)]
        [InlineData("varchar", DbType.AnsiString)]
        [InlineData("xml", DbType.Xml)]
        public void ParseTypeShouldReturnsDbType(string sqlDbType, DbType expectedDbType)
        {
            // Act
            DbType actualDbType = _typeReader.ParseType(sqlDbType);

            // Assert
            Assert.Equal(expectedDbType, actualDbType);
        }

        [Fact]
        public void ParseTypeThrowsArgumentOutOfRangeExpcetionWhenSqlTypeCannotBeMapped()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _typeReader.ParseType("whateverNotMappedValue")
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ParseTypeThrowsArgumentExceptionWhenSqlTypeIsNullOrWhiteSpace(string sqlType)
        {
            Assert.Throws<ArgumentException>(
                () => _typeReader.ParseType(sqlType)
            );
        }
    }
}
