using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.Helpers
{
    public static class IQueryableHelpers
    {
        public static ObjectContext GetObjectContext<T>(IQueryable<T> queryable) where T : class
        {
            MethodCallExpression methodCallExpression = (queryable.Expression as MethodCallExpression)
                ?? throw new InvalidOperationException($"The parameter \"{nameof(queryable)}\" hasn't been create from EntityFramework's DbSet.");


            ConstantExpression constantExpression 
                = methodCallExpression?.Object as ConstantExpression
                    ?? (methodCallExpression.Arguments[0] as MethodCallExpression)?.Object as ConstantExpression; // TODO remove that crap

            ObjectQuery objectQuery = constantExpression?.Value as ObjectQuery;

            return objectQuery?.Context
                ?? throw new InvalidOperationException($"The parameter \"{nameof(queryable)}\" hasn't been created from a DbSet");
        }
    }
}
