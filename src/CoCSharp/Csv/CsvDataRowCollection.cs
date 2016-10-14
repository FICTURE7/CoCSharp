using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataRow"/>.
    /// Base class of <see cref="CsvDataRowCollection{TCsvData}"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public abstract class CsvDataRowCollection : ICollection<CsvDataRow>
    {
        #region Constructors
        internal CsvDataRowCollection(Type csvDataType, CsvDataTable table)
        {
            if (csvDataType == null)
                throw new ArgumentNullException(nameof(csvDataType));
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _table = table;
        }
        #endregion

        #region Fields & Properties
        private readonly CsvDataTable _table;

        bool ICollection<CsvDataRow>.IsReadOnly => false;

        // Table which contains this collection.
        internal CsvDataTable Table => _table;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CsvDataRow this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index > Count - 1)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return GetRowByIndex(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CsvDataRow this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                return GetRowByName(name);
            }
        }

        /// <summary>
        /// Gets the number of <see cref="CsvDataRow"/> in the <see cref="CsvDataRowCollection"/>.
        /// </summary>
        public abstract int Count { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public abstract void Add(CsvDataRow row);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public abstract bool Contains(CsvDataRow row);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public abstract void CopyTo(CsvDataRow[] array, int arrayIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public abstract bool Remove(CsvDataRow row);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        public abstract void InsertAt(CsvDataRow row, int index);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<CsvDataRow> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Needed to get the indexer working.
        internal abstract CsvDataRow GetRowByIndex(int index);

        // Needed to get the indexer working.
        internal abstract CsvDataRow GetRowByName(string name);

        internal static CsvDataRowCollection CreateInternal(Type csvDataType, CsvDataTable table)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic;

            var ntype = typeof(CsvDataRowCollection<>).MakeGenericType(csvDataType);
            return (CsvDataRowCollection)Activator.CreateInstance(ntype, bindingAttr: BINDING_FLAGS, args: new object[] { table }, binder: null, culture: null);
        }
        #endregion
    }
}
