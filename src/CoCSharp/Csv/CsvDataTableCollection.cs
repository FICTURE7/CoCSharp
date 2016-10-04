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
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataTableCollection"/> class.
        /// </summary>
        public CsvDataTableCollection()
        {
            _tables = new CsvDataTable[32];
        }

        #region Fields & Properties
        bool ICollection<CsvDataTable>.IsReadOnly { get { return false; } }

        private CsvDataTable[] _tables;
        /// <summary>
        /// Gets the <see cref="CsvDataTable"/> at the specified table index.
        /// </summary>
        /// <param name="tableIndex">Zero-based index of the <see cref="CsvDataTable"/> to locate.</param>
        /// <returns>Instance of the <see cref="CsvDataTable"/> if found; otherwise, null.</returns>
        public CsvDataTable this[int tableIndex]
        {
            get
            {
                if (tableIndex < 0 || tableIndex > Count)
                    throw new ArgumentOutOfRangeException(nameof(tableIndex), "Index must be non-negative and less than Count.");

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
        public void Add(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            AddInternal(table);
        }

        bool ICollection<CsvDataTable>.Remove(CsvDataTable item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CsvDataTable>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<CsvDataTable>.Contains(CsvDataTable item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CsvDataTable>.CopyTo(CsvDataTable[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<CsvDataTable> IEnumerable<CsvDataTable>.GetEnumerator()
        {
            throw new NotImplementedException();
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

        private void AddInternal(CsvDataTable item)
        {
            var index = item.KindID;
            var arrayLength = _tables.Length - 1;
            if (index > arrayLength)
                Array.Resize(ref _tables, index + 4);

            _tables[index] = item;
            _count++;
        }

        private bool RemoveInternal(int kindId)
        {
            var index = kindId;
            if (index > _tables.Length - 1)
                return false;

            return true;
        }
    }
    #endregion
}
