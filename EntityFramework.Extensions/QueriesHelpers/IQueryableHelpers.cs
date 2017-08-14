using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions.QueriesHelpers
{
    public static class IQueryableHelpers
    {
        public static ObjectContext GetObjectContext<T>(IQueryable<T> queryable) where T : class
        {
            MethodCallExpression methodCallExpression = queryable.Expression as MethodCallExpression;

            ConstantExpression constantExpression = methodCallExpression?.Object as ConstantExpression;

            ObjectQuery objectQuery = constantExpression?.Value as ObjectQuery;

            return objectQuery?.Context
                ?? throw new InvalidOperationException($"The parameter \"{nameof(queryable)}\" hasn't been created from a DbSet");
        }
    }
}
