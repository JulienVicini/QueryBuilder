namespace QueryBuilder.Core.Mappings
{
    public interface IDataTransformer<TSource, TResult>
    {
        TResult Transform(TSource sourceData);
    }
}
