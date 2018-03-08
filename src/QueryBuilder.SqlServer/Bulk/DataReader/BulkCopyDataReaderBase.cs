using QueryBuilder.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace QueryBuilder.SqlServer.Bulk.DataReader
{
    /// <summary>
    /// IDataReader abstract implement.
    /// This class throws NotImplementException on each unncessary methods to process a BulkCopy.
    /// This class is used only to make the final implement cleaner
    /// </summary>
    public abstract class BulkCopyDataReaderBase: IBulkDataReader
    {
        public BulkCopyDataReaderBase(IEnumerable<string> columns)
        {
            ThrowHelper.ThrowIfNullOrEmpty(columns, nameof(columns));

            Columns  = new ReadOnlyCollection<string>(columns.ToList());
            IsClosed = false;
        }

        public IReadOnlyCollection<string> Columns { get; private set; }
        public int FieldCount => Columns.Count;
        public int GetOrdinal(string name)
        {
            ThrowHelper.ThrowIfNullOrWhiteSpace(name, nameof(name));

            int index = 0;

            using(IEnumerator<string> columnEnumerator = Columns.GetEnumerator())
            {
                while (columnEnumerator.MoveNext())
                    if (columnEnumerator.Current == name) return index;
                    else index++;
            }

            throw new ArgumentOutOfRangeException();
        }

        public bool IsClosed { get; private set; }
        public virtual void Close() => IsClosed = true;

        #region Abstract Method / Members

        public abstract bool Read();
        public abstract object GetValue(int i);
        public abstract void Dispose();

        #endregion

        #region Method not necessary for bulk copy

        public DataTable GetSchemaTable() => throw new NotImplementedException();
        public object this[int i] => throw new NotImplementedException();
        public object this[string name] => throw new NotImplementedException();
        public int Depth => throw new NotImplementedException();
        public int RecordsAffected => throw new NotImplementedException();
        public bool NextResult() => throw new NotImplementedException();

        public bool IsDBNull(int i) => throw new NotImplementedException();
        public int GetValues(object[] values) => throw new NotImplementedException();
        public bool GetBoolean(int i) => throw new NotImplementedException();
        public byte GetByte(int i) => throw new NotImplementedException();
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public char GetChar(int i) => throw new NotImplementedException();
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public IDataReader GetData(int i) => throw new NotImplementedException();
        public string GetDataTypeName(int i) => throw new NotImplementedException();
        public DateTime GetDateTime(int i) => throw new NotImplementedException();
        public decimal GetDecimal(int i) => throw new NotImplementedException();
        public double GetDouble(int i) => throw new NotImplementedException();
        public Type GetFieldType(int i) => throw new NotImplementedException();
        public float GetFloat(int i) => throw new NotImplementedException();
        public Guid GetGuid(int i) => throw new NotImplementedException();
        public short GetInt16(int i) => throw new NotImplementedException();
        public int GetInt32(int i) => throw new NotImplementedException();
        public long GetInt64(int i) => throw new NotImplementedException();
        public string GetName(int i) => throw new NotImplementedException();
        public string GetString(int i) => throw new NotImplementedException();

        #endregion
    }
}
