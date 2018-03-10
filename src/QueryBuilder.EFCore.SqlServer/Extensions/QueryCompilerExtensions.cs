using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QueryBuilder.Core.Helpers;

namespace QueryBuilder.EFCore.SqlServer.Extensions
{
    using Microsoft.EntityFrameworkCore.Storage;

    public static class QueryCompilerExtensions
    {
        private static readonly PropertyInfo Database_Property = typeof(QueryCompiler).GetProperty("Database", BindingFlags.Instance | BindingFlags.NonPublic);

        public static Database GetDatabase(this QueryCompiler entityQueryProvider)
        {
            Check.NotNull(entityQueryProvider, nameof(entityQueryProvider));

            return (Database)Database_Property.GetValue(entityQueryProvider);
        }
    }
}
