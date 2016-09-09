using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Base <see cref="CsvDataRow{TCsvData}"/> class. It is not intended to be used.
    /// </summary>
    [DebuggerTypeProxy(typeof(CsvDataRowDebugView))]
    public abstract class CsvDataRow : ICollection<CsvDataCollection>
    {
        /// <summary>
        /// Value indicating the maximum width of a <see cref="CsvDataRow"/>. This field is constant.
        /// </summary>
        public const int MaxWidth = 999999;

        internal CsvDataRow()
        {
            // Space
        }

        internal abstract int KindID { get; }
       
        public abstract int Count { get; }

        // Needed for the CsvDataCollectionRef to work properly.
        internal abstract CsvDataCollection GetByIndex(int index);

        // Returns a new instance of the CsvDataTable<> class with the specified type as generic parameter.
        internal static CsvDataRow CreateInstance(Type type)
        {
            var ntype = typeof(CsvDataRow<>).MakeGenericType(type);
            return (CsvDataRow)Activator.CreateInstance(ntype);
        }

        #region ICollection<CsvDataCollection>
        bool ICollection<CsvDataCollection>.IsReadOnly => false;

        public abstract void Add(CsvDataCollection dataCollection);

        public abstract void Insert(int index, CsvDataCollection dataCollection);

        public abstract void Clear();

        public abstract bool Contains(CsvDataCollection dataCollection);

        public abstract void CopyTo(CsvDataCollection[] array, int arrayIndex);

        public abstract bool Remove(CsvDataCollection dataCollection);

        public abstract IEnumerator<CsvDataCollection> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
