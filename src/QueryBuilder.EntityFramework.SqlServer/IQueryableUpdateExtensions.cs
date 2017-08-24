using QueryBuilder.Core.Statements;
using QueryBuilder.EntityFramework.Database;
using QueryBuilder.EntityFramework.IQueryable;
using QueryBuilder.EntityFramework.Mappings;
using QueryBuilder.EntityFramework.SqlServer.FluentSyntax;
using QueryBuilder.SqlServer.Statements;
using System;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.EntityFramework.SqlServer
{
    public static class IQueryableUpdateExtensions
    {
        public static UpdateStatementBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, Expression<Func<T, TValue>> valueExpression)
        {
            return new UpdateStatementBuilder<T>(queryable)
                        .SetValue(memberExpression, valueExpression);
        }

        public static UpdateStatementBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, TValue value)
        {
            return new UpdateStatementBuilder<T>(queryable)
                        .SetValue(memberExpression, value);
        }

        public static int Update<T>(this UpdateStatementBuilder<T> updateBuilder) where T : class
        {
            IQueryable<T> queryable = updateBuilder.Queryable;

            // Create Query
            var query = new UpdateStatement<T>(
                updateBuilder.Assignements,
                IQueryableHelpers.GetQueryPredicate(queryable)
            );

            // Get info from IQueryable
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            var objectContextAdapter = new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(objectContext);

            var mappingAdapter = new EntityTypeMappingAdapter<T>(objectContext.GetEntityMetaData<T>());

            // Create Query
            var orchestrator = new StatementFacade<T>(
                new SqlQueryTranslator<T>(mappingAdapter),
                objectContextAdapter
            );

            return orchestrator.Update(query);
        }
    }
}
