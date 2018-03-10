using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QueryBuilder.EFCore.SqlServer.Extensions;
using System;
using System.Linq;

namespace QueryBuilder.EFCore.SqlServer.Helpers
{
    public static class IQueryableHelper
    {
        public static DbContext GetDbContext<T>(IQueryable<T> queryable) where T : class
        {
            EntityQueryProvider queryProvider 
                = queryable.Provider as EntityQueryProvider
                    ?? throw new ArgumentException(nameof(queryable), $"The \"{typeof(IQueryable<>).FullName}\" is not of type \"{typeof(EntityQueryProvider).FullName}\".");

            return queryProvider.GetQueryCompiler()
                                .GetDatabase()
                                .GetDependencies()
                                .QueryCompilationContextFactory
                                .GetDependencies()
                                .CurrentContext.Context;
        }
    }
}
