using QueryBuilder.Core.Mappings;
using System;
using System.Data;

namespace QueryBuilder.SqlServer.Statements
{
    public static class SqlTypeTranslator
    {
        // https://msdn.microsoft.com/fr-fr/library/ms187752(v=sql.120).aspx
        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
        public static string TranslateType<TEntity>(ColumnMapping<TEntity> columnMapping)
            where TEntity : class
        {
            switch (columnMapping.DbType)
            {
                // Exact Numeric values
                case DbType.Byte   : return "tinyint";
                case DbType.SByte  :
                case DbType.Int16  : return "smallint";
                case DbType.Int32  : return "int";
                case DbType.Int64  : return "bigint";
                case DbType.UInt16 : return "int";
                case DbType.UInt32 : return "bigint";
                case DbType.UInt64 : return "NUMERIC(20)";
                case DbType.Decimal: return $"decimal({columnMapping.Precision}, {columnMapping.Scale})";
                case DbType.Boolean: return "bit";
                // TODO MONEY

                // Floating
                case DbType.Double: return "float";
                case DbType.Single: return "Real";

                // Date
                case DbType.Date          : return "date";
                case DbType.DateTime      : return "datetime";
                case DbType.DateTime2     : return "datetime2";
                case DbType.DateTimeOffset: return "datetimeoffset";
                case DbType.Time          : return "time";

                // Strings
                case DbType.AnsiString           : return $"varchar({columnMapping.Length?.ToString() ?? "max"})";
                case DbType.AnsiStringFixedLength: return $"char({columnMapping.Length?.ToString() ?? "max"})";
                case DbType.String               : return $"nvarchar({columnMapping.Length?.ToString() ?? "max"})";
                case DbType.StringFixedLength    : return $"nchar({columnMapping.Length?.ToString() ?? "max"})";

                // Binary
                case DbType.Binary               : return $"varbinary({columnMapping.Length?.ToString() ?? "max"})";

                // OTHER
                case DbType.Object: return "sql_variant";
                case DbType.Guid  : return "uniqueidentifier";
                case DbType.Xml   : return "xml";
                
                case DbType.Currency  :
                case DbType.VarNumeric:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
