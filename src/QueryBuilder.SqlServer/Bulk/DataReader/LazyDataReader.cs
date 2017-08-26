using QueryBuilder.Helpers;
using System.Collections.Generic;

namespace QueryBuilder.SqlServer.Bulk.DataReader
{
    public class LazyDataReader<T>
        : BulkCopyDataReaderBase
        where T : class
    {
        #region Members

        private readonly IEnumerator<T> _enumerator;
        private readonly OrdinalValueLookup<T> _ordinalValueAccessor;

        #endregion

        #region Constructors

        public LazyDataReader(IEnumerable<T> items, IReadOnlyCollection<string> columns, OrdinalValueLookup<T> ordinalValueLookup)
            : base(columns)
        {
            ThrowHelper.ThrowIfNullOrEmpty(items, nameof(items));


            _enumerator           = items.GetEnumerator();
            _ordinalValueAccessor = ordinalValueLookup;
        }

        #endregion


        #region GetData

        public override bool Read() => _enumerator.MoveNext();

        public override object GetValue(int i) => _ordinalValueAccessor(_enumerator.Current, i);

        #endregion

        public override void Dispose()
        {
            base.Close();
            _enumerator?.Dispose();
        }
    }
}