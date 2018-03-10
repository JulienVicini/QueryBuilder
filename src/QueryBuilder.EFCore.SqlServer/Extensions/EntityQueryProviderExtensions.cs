using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QueryBuilder.Core.Helpers;

namespace QueryBuilder.EFCore.SqlServer.Extensions
{
    public static class EntityQueryProviderExtensions
    {
        private static readonly FieldInfo QueryCompiler_FieldInfo = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.Instance | BindingFlags.NonPublic);

        public static QueryCompiler GetQueryCompiler(this EntityQueryProvider entityQueryProvider)
        {
            ThrowHelper.ThrowIfNull(entityQueryProvider, nameof(entityQueryProvider));

            return (QueryCompiler)QueryCompiler_FieldInfo.GetValue(entityQueryProvider);
        }
    }
}
