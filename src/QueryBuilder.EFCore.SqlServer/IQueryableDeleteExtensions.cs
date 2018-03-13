using System;
using System.Linq;
using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core.IQueryables;
using QueryBuilder.EFCore.SqlServer.Factories;
using QueryBuilder.Core.Services;

namespace QueryBuilder.EFCore.SqlServer
{
    public static class IQueryableDeleteExtensions
    {
        public static int Delete<T>(this IQueryable<T> queryable)
            where T : class
        {
            // Resolve service
            var serviceFactory = new ServiceFactory<T>(queryable);
            ICommandService<T> commandService = serviceFactory.CreateCommandService();

            // Execute Query
            var query = new DeleteStatement<T>(IQueryableHelper.GetQueryPredicate(queryable));

            return commandService.Delete(query);
        }
    }
}
