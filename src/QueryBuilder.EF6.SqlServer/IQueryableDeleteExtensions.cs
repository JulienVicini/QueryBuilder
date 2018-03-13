using QueryBuilder.Core.IQueryables;
using QueryBuilder.Core.Services;
using QueryBuilder.Core.Statements;
using QueryBuilder.EF6.SqlServer.Factories;
using System.Linq;

namespace QueryBuilder.EF6.SqlServer
{
    public static class IQueryableDeleteExtensions
    {
        public static int Delete<T>(this IQueryable<T> queryable)
            where T : class
        {
            // Resolve Service
            var serviceFactory = new ServiceFactory<T>(queryable);
            ICommandService<T> commandService = serviceFactory.CreateCommandService();

            // Create & Execute Query
            var query = new DeleteStatement<T>(IQueryableHelper.GetQueryPredicate(queryable));

            return commandService.Delete(query);
        }
    }
}
