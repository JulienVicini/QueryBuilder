using System;
using System.Linq.Expressions;

namespace QueryBuilder.SqlServer.Statements
{
    public static class SqlOperatorTranslator
    {
        public static string GetOpertor(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Assign:
                case ExpressionType.Equal: return "=";
                //case ExpressionType.And:
                case ExpressionType.AndAlso: return "AND";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.LessThanOrEqual: return "<=";
                case ExpressionType.NotEqual: return "<>";
                //case ExpressionType.Or:
                case ExpressionType.OrElse: return "OR";

                // Arithmeic Operator
                case ExpressionType.Add     : return "+";
                case ExpressionType.Subtract: return "-";
                case ExpressionType.Divide  : return "/";
                case ExpressionType.Multiply: return "*";
                case ExpressionType.Modulo  : return "%";

                default:
                    throw new NotSupportedException($"Cannot convert {expressionType} to SQL Operator.");
            }
        }
    }
}
