namespace EntityFramework.Extensions.Core.Queries
{
    public interface IQueryTranslator<TEntity>
        where TEntity : class
    {
        void TranslateUpdateQuery(UpdateQuery<TEntity> updateQuery);

        void TranslateDeleteQuery(DeleteQuery deleteQuery);
    }
}
