using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataTable"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvDataTableCollection : ICollection<CsvDataTable>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataTableCollection"/> class.
        /// </summary>
        public CsvDataTableCollection()
        {
            _tables = new CsvDataTable[32];
        }
        #endregion

        #region Fields & Properties
        private CsvDataTable[] _tables;

        bool ICollection<CsvDataTable>.IsReadOnly => false;

        /// <summary>
        /// Gets the <see cref="CsvDataTable"/> at the specified table index.
        /// </summary>
        /// <param name="tableIndex">Zero-based index of the <see cref="CsvDataTable"/> to locate.</param>
        /// <returns>Instance of the <see cref="CsvDataTable"/> if found; otherwise, null.</returns>
        public CsvDataTable this[int tableIndex]
        {
            get
            {
                if (tableIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(tableIndex), "Index must be non-negative and less than Count.");

                if (tableIndex >= _tables.Length)
                    return null;

                return _tables[tableIndex];
            }
        }

        private int _count;
        /// <summary>
        /// Gets the number of <see cref="CsvDataTable"/> in the <see cref="CsvDataTableCollection"/>.
        /// </summary>
        public int Count => _count;
        #endregion

        #region Methods
        #region ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public void Add(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            AddInternal(table);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public bool Remove(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            return RemoveInternal(table);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _tables.Length; i++)
                _tables[i] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool Contains(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            return ContainsInternal(table);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CsvDataTable[] array, int arrayIndex)
        {
            _tables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CsvDataTable> GetEnumerator()
        {
            for (int i = 0; i < _tables.Length; i++)
                if (_tables[i] != null) yield return _tables[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Returns the <see cref="CsvDataTable{TCsvData}"/> for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="CsvData"/>.</typeparam>
        /// <returns><see cref="CsvDataTable{TCsvData}"/> for the specified type; returns null if not loaded.</returns>
        public CsvDataTable<T> GetTable<T>() where T : CsvData, new()
        {
            var kindId = CsvData.GetKindID(typeof(T));
            var table = _tables[kindId];
            var castedTable = table as CsvDataTable<T>;
            Debug.Assert(castedTable != null, "KindID associated with type failed to cast.");
            return castedTable;
        }

        /// <summary>
        /// Returns the <see cref="CsvDataTable"/> for the specified type.
        /// </summary>
        /// <param name="type">Type of <see cref="CsvData"/>.</param>
        /// <returns><see cref="CsvDataTable"/> for the specified type; returns null if not loaded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="type"/> is abstract or is does not inherit from <see cref="CsvData"/>.</exception>
        public CsvDataTable GetTable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsAbstract || type.BaseType != typeof(CsvData))
                throw new ArgumentException("type must be inherited from CsvData and non-abstract.", nameof(type));

            var kindId = CsvData.GetKindID(type);
            return _tables[kindId];
        }

        private void AddInternal(CsvDataTable table)
        {
            var index = table.KindID;
            // Resize the array to avoid exceptions.
            if (index >= _tables.Length)
                Array.Resize(ref _tables, index + 4);

            if (_tables[index] != null)
                throw new ArgumentException("Already contain a table of same type.", nameof(table));

            _tables[index] = table;
            _count++;
        }

        private bool RemoveInternal(CsvDataTable table)
        {
            var index = table.KindID;
            if (!ContainsInternal(table))
                return false;

            _tables[index] = null;
            _count--;
            return true;
        }

        private bool ContainsInternal(CsvDataTable table)
        {
            var index = table.KindID;
            if (index >= _tables.Length)
                return false;

            if (_tables[index] == null)
                return false;

            return true;
        }
    }
    #endregion
}
