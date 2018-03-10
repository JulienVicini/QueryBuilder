using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using QueryBuilder.Core.Helpers;

namespace QueryBuilder.EFCore.Mappings
{
    public class RelationalSqlTypePropertyTranslator
    {
        public const string REGEX_PATTERN = @"([A-Z]+)(\([\s]*([0-9]+)[\s]*(,[\s]*([0-9]+)[\s]*)?\))?";
        public readonly Regex REGEX = new Regex(REGEX_PATTERN, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void GetMetaData(string sqlType, out int? precision, out int? scale, out DbType dbType)
        {
            Check.NotNullOrWhiteSpace(sqlType, nameof(sqlType));


            Match match = REGEX.Match(sqlType);

            if (match.Success)
            {
                dbType = ParseType(match.Groups[1].Value);

                if (dbType == DbType.Decimal)
                {
                    precision = ParseGroup(match.Groups[3]);
                    scale     = ParseGroup(match.Groups[5]);
                }
                else
                {
                    precision = scale = null;
                }
            }
            else
                throw new ArgumentException(nameof(sqlType), $"The Sql Type \"{sqlType}\" cannot be parsed.");
        }

        // from https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
        public DbType ParseType(string sqlType)
        {
            Check.NotNullOrWhiteSpace(sqlType, nameof(sqlType));

            switch (sqlType.ToLower())
            {
                case "bigint"          : return DbType.Int64;
                case "binary"          : return DbType.Binary;
                case "bit"             : return DbType.Boolean;
                case "char"            : return DbType.AnsiStringFixedLength;
                case "date"            : return DbType.Date;
                case "datetime"        : return DbType.DateTime;
                case "datetime2"       : return DbType.DateTime2;
                case "datetimeoffset"  : return DbType.DateTimeOffset;
                case "decimal"         : return DbType.Decimal;
//FILESTREAM attribute(varbinary(max))   Binary
                case "float"           : return DbType.Double;
                case "image"           : return DbType.Binary;
                case "int"             : return DbType.Int32;
                case "money"           : return DbType.Decimal;
                case "nchar"           : return DbType.StringFixedLength;
                case "ntext"           : return DbType.String;
                case "numeric"         : return DbType.Decimal;
                case "nvarchar"        : return DbType.String;
                case "real"            : return DbType.Single;
                case "rowversion"      : return DbType.Binary;
                case "smalldatetime"   : return DbType.DateTime;
                case "smallint"        : return DbType.Int16;
                case "smallmoney"      : return DbType.Decimal;
                case "sql_variant"     : return DbType.Object;
                case "text"            : return DbType.String;
                case "time"            : return DbType.Time;
                case "timestamp"       : return DbType.Binary;
                case "tinyint"         : return DbType.Byte;
                case "uniqueidentifier": return DbType.Guid;
                case "varbinary"       : return DbType.Binary;
                case "varchar"         : return DbType.AnsiString;
                case "xml"             : return DbType.Xml;

                default:
                    throw new ArgumentOutOfRangeException(nameof(sqlType), $"The sql type\"{sqlType}\" cannot be mapped to \"{typeof(DbType).FullName}\".");
            }
        }

        public int ParseGroup(Group group)
        {
             return int.Parse(group.Value);
        }
    }
}
