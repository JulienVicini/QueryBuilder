using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryBuilder.EFCore.SqlServer.Helpers
{
    public static class IQueryableHelper
    {
        private static readonly FieldInfo QueryProvider_QueryCompoiler_Field;
        private static readonly PropertyInfo QueryCompiler_Database_Property;
        private static readonly PropertyInfo RelationalDatabase_Dependencies_Property;

        static IQueryableHelper()
        {
            QueryProvider_QueryCompoiler_Field  = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.Instance | BindingFlags.NonPublic);
            QueryCompiler_Database_Property = typeof(QueryCompiler).GetProperty("Database", BindingFlags.Instance | BindingFlags.NonPublic);
            RelationalDatabase_Dependencies_Property = typeof(Microsoft.EntityFrameworkCore.Storage.RelationalDatabase).GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static DbContext GetDbContext<T>(IQueryable<T> queryable) where T : class
        {
            // TODO refactor
            ConstantExpression constantExpression = queryable.Expression as ConstantExpression;


            EntityQueryable<T> entityQueryable = constantExpression?.Value as EntityQueryable<T>;

            EntityQueryProvider queryProvider = (EntityQueryProvider)entityQueryable.Provider;
            var queryCompiler = (QueryCompiler)QueryProvider_QueryCompoiler_Field.GetValue(queryProvider);

            var database = (Microsoft.EntityFrameworkCore.Storage.RelationalDatabase) QueryCompiler_Database_Property.GetValue(queryCompiler);

            var dependencies = (Microsoft.EntityFrameworkCore.Storage.DatabaseDependencies)RelationalDatabase_Dependencies_Property.GetValue(database);


            var compilation = (SqlServerQueryCompilationContextFactory)dependencies.QueryCompilationContextFactory;

            var queryCompolationDependencies = (QueryCompilationContextDependencies) compilation.GetType().GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.NonPublic)
                                                                                          .GetValue(compilation);

            return queryCompolationDependencies.CurrentContext.Context;
        }
    }
}
