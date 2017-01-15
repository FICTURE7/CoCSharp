using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataColumn"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvDataColumnCollection : ICollection<CsvDataColumn>
    {
        #region Constructors
        internal CsvDataColumnCollection(Type csvDataType, CsvDataTable table)
        {
            if (csvDataType == null)
                throw new ArgumentNullException(nameof(csvDataType));
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _table = table;
            _columns = new List<CsvDataColumn>(16);
        }
        #endregion

        #region Fields & Properties
        private readonly CsvDataTable _table;
        private readonly List<CsvDataColumn> _columns;

        bool ICollection<CsvDataColumn>.IsReadOnly => false;

        /// <summary>
        /// Gets the <see cref="CsvDataTable"/> to which this <see cref="CsvDataColumnCollection"/> belongs
        /// to.
        /// </summary>
        internal CsvDataTable Table => _table;

        /// <summary>
        /// Gets the number of <see cref="CsvDataColumn"/> in the <see cref="CsvDataColumnCollection"/>.
        /// </summary>
        public int Count => _columns.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public CsvDataColumn this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index must be non-negative.");

                if (columnIndex >= _columns.Count)
                    return null;
                return _columns[columnIndex];
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        public void Add(CsvDataColumn column)
        {
            CheckColumn(column);

            InternalInsert(column, _columns.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _columns.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool Contains(CsvDataColumn column)
        {
            CheckColumn(column);

            return _columns.Contains(column);
        }

        /// <summary>
        /// Copies the entire collection into an existing array, starting at a specified index within the array. 
        /// </summary>
        /// <param name="array">An array of <see cref="CsvDataColumn" /> objects to copy the collection into.></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CsvDataColumn[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            _columns.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified <see cref="CsvDataColumn"/> from the collection.
        /// </summary>
        /// <param name="column">The <see cref="CsvDataColumn"/> to remove.</param>
        /// <returns><c>true</c> if success; otherwise; <c>false</c>.</returns>
        public bool Remove(CsvDataColumn column)
        {
            CheckColumn(column);
            var index = _columns.IndexOf(column);
            if (index == -1)
                return false;

            column._table = null;
            column._dataLevel = -1;
            for (; index < _columns.Count; index++)
                _columns[index]._dataLevel = index;

            _columns.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataColumnCollection"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="CsvDataColumnCollection"/>.</returns>
        public IEnumerator<CsvDataColumn> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void InternalInsert(CsvDataColumn column, int index)
        {
            _columns.Insert(index, column);
            column._dataLevel = index;
            column._table = Table;
            for (; index < _columns.Count; index++)
                _columns[index]._dataLevel = index;
        }

        private void CheckColumn(CsvDataColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));
            if (column.Table != null)
                throw new ArgumentException("Column already belongs to a table.", nameof(column));
        }
        #endregion
    }
}
