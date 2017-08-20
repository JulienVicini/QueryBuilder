using EntityFramework.Extensions.Core.Mappings;
using System;

namespace EntityFramework.Extensions.SqlServer.Queries
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
                case System.Data.DbType.Byte   : return "tinyint";
                case System.Data.DbType.SByte  :
                case System.Data.DbType.Int16  : return "smallint";
                case System.Data.DbType.Int32  : return "int";
                case System.Data.DbType.Int64  : return "bigint";
                case System.Data.DbType.UInt16 : return "int";
                case System.Data.DbType.UInt32 : return "bigint";
                case System.Data.DbType.UInt64 : return "NUMERIC(20)";
                case System.Data.DbType.Decimal: return $"decimal({columnMapping.Precision}, {columnMapping.Scale})";
                case System.Data.DbType.Boolean: return "bit";
                // TODO MONEY

                // Floating
                case System.Data.DbType.Double: return "float";
                case System.Data.DbType.Single: return "Real";

                // Date
                case System.Data.DbType.Date          : return "date";
                case System.Data.DbType.DateTime      : return "datetime";
                case System.Data.DbType.DateTime2     : return "datetime2";
                case System.Data.DbType.DateTimeOffset: return "datetimeoffset";
                case System.Data.DbType.Time          : return "time";

                // Strings
                case System.Data.DbType.AnsiString           : return $"varchar({columnMapping.Length?.ToString() ?? "max"})";
                case System.Data.DbType.AnsiStringFixedLength: return $"char({columnMapping.Length?.ToString() ?? "max"})";
                case System.Data.DbType.String               : return $"nvarchar({columnMapping.Length?.ToString() ?? "max"})";
                case System.Data.DbType.StringFixedLength    : return $"nchar({columnMapping.Length?.ToString() ?? "max"})";

                // Binary
                case System.Data.DbType.Binary               : return $"varbinary({columnMapping.Length?.ToString() ?? "max"})";

                // OTHER
                case System.Data.DbType.Object: return "sql_variant";
                case System.Data.DbType.Guid  : return "uniqueidentifier";
                case System.Data.DbType.Xml   : return "xml";
                
                case System.Data.DbType.Currency  :
                case System.Data.DbType.VarNumeric:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
