namespace EntityFramework.Extensions.Core.BulkCopy
{
    public interface IDataTransformer<TSource, TResult>
    {
        TResult Transform(TSource sourceData);
    }
}
