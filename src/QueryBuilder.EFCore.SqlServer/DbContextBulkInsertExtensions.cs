using QueryBuilder.Core.Bulk;
using QueryBuilder.EFCore.SqlServer.Factories;
using QueryBuilder.SqlServer.Bulk.DataReader;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.Statements;
using QueryBuilder.SqlServer.Bulk;

namespace QueryBuilder.EFCore.SqlServer
{
    public static class DbContextBulkInsertExtensions
    {
        public static void BulkInsert<T>(this DbSet<T> dbSet, IEnumerable<T> data)
            where T : class
        {
            BulkService<T, IBulkDataReader> bulkCopy
                = new BulkFacadeFactory<T>().CreateBulkCopy(dbSet);

            bulkCopy.WriteToServer(data);
        }

        public static void BulkMerge<TEntity, TColumns>( this DbSet<TEntity> dbSet, IEnumerable<TEntity> data, Expression<Func<TEntity, TColumns>> mergeOnColumns, bool updateOnly = false)
            where TEntity : class
        {
            var databaseContext = new DatabaseAdapterFactory<TEntity>().CreateDatabaseContext(dbSet);
            IMappingAdapter<TEntity> mappingAdapter = new MappingAdapterFactory<TEntity>().CreateMappingAdapter(dbSet);

            IEnumerable<MemberExpression> mergeKeys = ExpressionHelper.GetSelectedMemberInAnonymousType(mergeOnColumns);

            string temporaryTable = "#tmp_bulk";

            // TODO change update or insert selection
            var mergeQuery = new MergeStatement<TEntity>(mergeKeys, temporaryTable, updateOnly ? MergeStatement<TEntity>.MergeType.UpdateOnly : MergeStatement<TEntity>.MergeType.InsertOrUpdate);
            var queryFacade = new StatementFacadeFactory<TEntity>().CreateFacade(dbSet);

            // TODO put in factory
            var bulkCopy = new BulkService<TEntity, IBulkDataReader>(
                bulkExecutor: new SqlBulkMergeExecutor<TEntity>(databaseContext, queryFacade, mergeQuery),
                dataTransformer: new DataReaderDataTransformer<TEntity>(mappingAdapter),
                mappingAdapter: mappingAdapter
            );

            bulkCopy.WriteToServer(data);
        }
    }
}
