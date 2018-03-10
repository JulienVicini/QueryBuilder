using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QueryBuilder.Core.Helpers;

namespace QueryBuilder.EFCore.SqlServer.Extensions
{
    public static class IQueryCompilationContextFactoryExtensions
    {
        private static readonly PropertyInfo Dependencies_Property = typeof(QueryCompilationContextFactory).GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.NonPublic);

        public static QueryCompilationContextDependencies GetDependencies(this IQueryCompilationContextFactory queryCompilationContextFactory)
        {
            Check.NotNull(queryCompilationContextFactory, nameof(queryCompilationContextFactory));

            return (QueryCompilationContextDependencies)Dependencies_Property.GetValue(queryCompilationContextFactory);
        }
    }
}
