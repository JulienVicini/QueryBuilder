using QueryBuilder.EFCore.SqlServer.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.Services;

namespace QueryBuilder.EFCore.SqlServer
{
    public static class DbContextBulkInsertExtensions
    {
        public static void BulkInsert<T>(this DbSet<T> dbSet, IEnumerable<T> data)
            where T : class
        {
            // resolve service
            var serviceFactory = new ServiceFactory<T>(dbSet);
            IBulkInsertService<T> bulkInsertService = serviceFactory.CreateBulkInsert();

            // perform bulk insert
            bulkInsertService.WriteToServer(data);
        }

        public static void BulkMerge<T, TColumns>( this DbSet<T> dbSet, IEnumerable<T> data, Expression<Func<T, TColumns>> mergeOnColumns, bool updateOnly = false)
            where T : class
        {
            // resolve service
            var serviceFactory = new ServiceFactory<T>(dbSet);
            IBulkMergeService<T> bulkMerge = serviceFactory.CreateBulkMerge();

            // perform bulk insert
            IEnumerable<MemberExpression> mergeKeys 
                = ExpressionHelper.GetSelectedMemberInAnonymousType(mergeOnColumns); // TODO move this operation inside service

            bulkMerge.WriteToServer(data, mergeKeys, updateOnly);
        }
    }
}
