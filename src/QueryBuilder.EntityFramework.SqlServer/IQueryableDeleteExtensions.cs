﻿using QueryBuilder.Core.Statements;
using QueryBuilder.EntityFramework.Database;
using QueryBuilder.EntityFramework.IQueryable;
using QueryBuilder.EntityFramework.Mappings;
using QueryBuilder.SqlServer.Statements;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace QueryBuilder.EntityFramework.SqlServer
{
    public static class IQueryableDeleteExtensions
    {
        public static int Delete<T>(this IQueryable<T> queryable)
            where T : class
        {
            // BuilderQuery
            var query = new DeleteStatement<T>(IQueryableHelpers.GetQueryPredicate(queryable));

            // Get info from IQueryable
            ObjectContext objectContext = IQueryableHelpers.GetObjectContext(queryable);

            var objectContextAdapter = new ObjectContextDatabaseAdapter<SqlConnection, SqlTransaction>(objectContext);

            var mappingAdapter = new EntityTypeMappingAdapter<T>(objectContext.GetEntityMetaData<T>());

            // Create Query
            var orchestrator = new StatementFacade<T>(
                new SqlQueryTranslator<T>(mappingAdapter),
                objectContextAdapter
            );

            return orchestrator.Delete(query);
        }
    }
}
