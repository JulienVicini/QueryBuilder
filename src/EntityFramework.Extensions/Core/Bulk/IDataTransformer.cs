namespace EntityFramework.Extensions.Core.Bulk
{
    public interface IDataTransformer<TSource, TResult>
    {
        TResult Transform(TSource sourceData);
    }
}
