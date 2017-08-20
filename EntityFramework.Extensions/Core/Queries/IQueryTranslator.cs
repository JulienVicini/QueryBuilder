using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Queries
{
    public interface IQueryTranslator<TEntity>
        where TEntity : class
    {
        (string, IEnumerable<object>) TranslateQuery(DeleteQuery<TEntity> query);

        (string, IEnumerable<object>) TranslateQuery(UpdateQuery<TEntity> query);

        (string, IEnumerable<object>) TranslateQuery(AlterTableQuery<TEntity> query);

        (string, IEnumerable<object>) TranslateQuery(MergeQuery<TEntity> query);
    }
}
