using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a strongly typed table of <see cref="CsvData"/> of the specified type, <typeparamref name="TCsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to store.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvDataTable<TCsvData> : CsvDataTable, ICollection<CsvDataRow<TCsvData>> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataTable{TCsvData}"/> class.
        /// </summary>
        public CsvDataTable()
        {
            _rows = new CsvDataRowCollection<TCsvData>();

            _kindId = CsvData.GetInstance<TCsvData>().KindID;
            _columns = new List<CsvDataRow<TCsvData>>(48);
            _columnName2columnIndex = new Dictionary<string, int>();
        }
        #endregion

        #region Fields & Properties

        private readonly CsvDataRowCollection<TCsvData> _rows;

        // List of TCsvData[] which represents the columns.
        private readonly List<CsvDataRow<TCsvData>> _columns;
        // Dictionary mapping columns names to columns indexes.
        private readonly Dictionary<string, int> _columnName2columnIndex;

        bool ICollection<CsvDataRow<TCsvData>>.IsReadOnly => false;

        /// <summary>
        /// Gets the number of <see cref="CsvDataRow{TCsvData}"/> in the <see cref="CsvDataTable{TCsvData}"/>.
        /// </summary>
        public int Count => _columns.Count;

        public CsvDataRowCollection<TCsvData> Rows => _rows;

        public CsvDataRow<TCsvData> this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0 || columnIndex > MaxWidth)
                    throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be non-negative and less than or equal to MaxWidth.");

                return _columns[columnIndex];
            }
        }

        public CsvDataRow<TCsvData> this[string columnName]
        {
            get
            {
                if (columnName == null)
                    throw new ArgumentNullException("columnName");

                var columnIndex = 0;
                if (!_columnName2columnIndex.TryGetValue(columnName, out columnIndex))
                    return null;

                return _columns[columnIndex];
            }
        }

        // Kind ID of TCsvData, also known as the row index.
        private readonly int _kindId;
        internal override int KindID => _kindId;
        #endregion

        #region Methods
        internal override void ProxyAdd(object row)
        {
            Add((CsvDataRow<TCsvData>)row);
        }

        internal override object[] GetAllColumns()
        {
            return _columns.ToArray();
        }

        internal override CsvDataRow GetByIndex(int index)
        {
            return this[index];
        }

        #region ICollection
        public void Add(CsvDataRow<TCsvData> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));
            if (row._ref != null)
                throw new ArgumentException("dataCollection is already in another CsvDataRow.", "dataCollection");

            // Can have duplicate column names.
            // E.g: obstacles.csv => Large Stone.
            if (!_columnName2columnIndex.ContainsKey(row.Name))
                _columnName2columnIndex.Add(row.Name, _columns.Count);
            else
                Debug.WriteLine("Duplicate column names added: {0}", args: row.Name);

            // Set index of dataCollection to last index in the row,
            // that way the dataCollection has a correct ID.
            row._ref = new CsvDataRowRef<TCsvData>(_kindId, _columns.Count);
            _columns.Add(row);
        }

        public bool Remove(CsvDataRow<TCsvData> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var dataRef = row._ref;
            if (dataRef == null)
                return false;

            var columnIndex = dataRef.ColumnIndex;
            return RemoveInternal(columnIndex);
        }

        public bool Remove(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > MaxWidth)
                throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be non-negative and less than or equal to MaxWidth.");

            return RemoveInternal(columnIndex);
        }

        public bool Remove(string columnName)
        {
            if (columnName == null)
                throw new ArgumentNullException("columnName");

            var columnIndex = default(int);
            if (!_columnName2columnIndex.TryGetValue(columnName, out columnIndex))
                return false;

            return RemoveInternal(columnIndex);
        }

        /// <summary>
        /// Removes all <see cref="CsvDataRow{TCsvData}"/> in the <see cref="CsvDataTable{TCsvData}"/>.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                var column = _columns[i];

                column._ref = null;
                _columns.RemoveAt(i);
            }
        }

        public bool Contains(CsvDataRow<TCsvData> dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException("dataCollection");

            var dataRef = dataCollection._ref;
            if (dataRef == null)
                return false;

            var columnIndex = dataRef.ColumnIndex;
            if (columnIndex == -1)
                return false;

            if (columnIndex > _columns.Count - 1)
                return false;

            return _columns[columnIndex] == dataCollection;
        }

        public bool Contains(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > MaxWidth)
                throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be non-negative and less than or equal to MaxWidth.");

            if (columnIndex > _columns.Count - 1)
                return false;

            return true;
        }

        public bool Contains(string columnName)
        {
            if (columnName == null)
                throw new ArgumentNullException("columnName");

            return _columnName2columnIndex.ContainsKey(columnName);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataTable"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CsvDataRow<TCsvData>> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        void ICollection<CsvDataRow<TCsvData>>.CopyTo(CsvDataRow<TCsvData>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }
        #endregion

        private bool RemoveInternal(int columnIndex)
        {
            if (columnIndex > _columns.Count - 1)
                return false;

            var column = _columns[columnIndex];
            // Mark it as its not a CsvDataRow.
            column._ref = null;

            _columnName2columnIndex.Remove(column.Name);
            _columns.RemoveAt(columnIndex);
            return true;
        }
        #endregion
    }
}
