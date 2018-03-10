namespace QueryBuilder.EFCore.SqlServer.Extensions
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore.Storage;
    using QueryBuilder.Core.Helpers;

    public static class DatabaseExtensions
    {
        private static readonly PropertyInfo Dependencies_Property = typeof(Database).GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.NonPublic);

        public static DatabaseDependencies GetDependencies(this Database database)
        {
            Check.NotNull(database, nameof(database));

            return (DatabaseDependencies)Dependencies_Property.GetValue(database);
        }
    }
}
