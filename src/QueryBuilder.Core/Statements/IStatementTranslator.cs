using System.Collections.Generic;

namespace QueryBuilder.Core.Statements
{
    public interface IStatementTranslator<TEntity>
        where TEntity : class
    {
        (string, IEnumerable<object>) TranslateQuery(DeleteStatement<TEntity> query);

        (string, IEnumerable<object>) TranslateQuery(UpdateStatement<TEntity> query);

        (string, IEnumerable<object>) TranslateQuery(AlterTableStatement<TEntity> query);

        (string, IEnumerable<object>) TranslateQuery(MergeStatement<TEntity> query);
    }
}
