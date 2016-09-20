using System.Collections;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    public abstract class CsvDataRowCollection : ICollection<CsvDataRow>
    {
        bool ICollection<CsvDataRow>.IsReadOnly => false;

        public abstract int Count { get; }

        public abstract void Add(CsvDataRow item);

        public abstract void Clear();

        public abstract bool Contains(CsvDataRow item);

        public abstract void CopyTo(CsvDataRow[] array, int arrayIndex);

        public abstract IEnumerator<CsvDataRow> GetEnumerator();

        public abstract bool Remove(CsvDataRow item);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
