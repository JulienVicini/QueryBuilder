using EntityFramework.Extensions.Core.Queries.Statements.Operators;
using System;

namespace EntityFramework.Extensions.SqlServer.Queries
{
    public static class SQLOperatorConverter
    {
        public static string Convert(ArithmeticOperator @operator)
        {
            switch (@operator)
            {
                case ArithmeticOperator.Add     : return "+";
                case ArithmeticOperator.Divide  : return "/";
                case ArithmeticOperator.Modulo  : return "%";
                case ArithmeticOperator.Multipy : return "*";
                case ArithmeticOperator.Subtract: return "-";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator));
            }
        }

        public static string Convert(ConditionalOperator @operator)
        {
            switch (@operator)
            {
                case ConditionalOperator.And: return "AND";
                case ConditionalOperator.Or : return "OR";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator));
            }
        }

        public static string Convert(ComparisonOperator @operator)
        {
            switch (@operator)
            {
                case ComparisonOperator.Equal              : return "=" ;
                case ComparisonOperator.NotEqual           : return "<>";
                case ComparisonOperator.GreatherThan       : return ">" ;
                case ComparisonOperator.GreatherThanOrEqual: return ">=";
                case ComparisonOperator.LowerThan          : return "<" ;
                case ComparisonOperator.LowerThanOrEqual   : return "<=";
                // TODO handle those operators
                case ComparisonOperator.Range              :
                case ComparisonOperator.StartWith          :
                case ComparisonOperator.EndWidth           :
                case ComparisonOperator.Contains           :
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator));
            }
        } 
    }
}
