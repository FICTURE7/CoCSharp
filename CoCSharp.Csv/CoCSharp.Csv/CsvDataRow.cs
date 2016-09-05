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
    public abstract class CsvDataRow
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

        // Proxy method to call CsvDataTable<>.Add.
        // This is needed for CsvConvert to add rows to returning table.
        internal abstract void ProxyAdd(object row);

        // Needed to get the CsvDataRowDebugView to work correctly.
        internal abstract object[] GetAllColumns();

        // Needed for the CsvDataCollectionRef to work properly.
        internal abstract CsvDataCollection GetByIndex(int index);

        // Returns a new instance of the CsvDataTable<> class with the specified type as generic parameter.
        internal static CsvDataRow CreateInstance(Type type)
        {
            var ntype = typeof(CsvDataRow<>).MakeGenericType(type);
            return (CsvDataRow)Activator.CreateInstance(ntype);
        }

        internal sealed class CsvDataRowDebugView
        {
            public CsvDataRowDebugView(CsvDataRow row)
            {
                if (row == null)
                    throw new ArgumentNullException("row");

                _row = row;
            }

            private readonly CsvDataRow _row;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object[] Columns
            {
                get
                {
                    return _row.GetAllColumns();
                }
            }
        }
    }

    /// <summary>
    /// Represents a row of <see cref="CsvData"/> in a <see cref="CsvDataTable"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to store.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvDataRow<TCsvData> : CsvDataRow, ICollection<CsvDataCollection<TCsvData>> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataRow{TCsvData}"/> class.
        /// </summary>
        public CsvDataRow()
        {
            _kindId = CsvData.GetInstance<TCsvData>().KindID;
            _columns = new List<CsvDataCollection<TCsvData>>(48);
            _columnName2columnIndex = new Dictionary<string, int>();
        }
        #endregion

        #region Fields & Properties
        // List of TCsvData[] which represents the columns.
        private readonly List<CsvDataCollection<TCsvData>> _columns;
        // Dictionary mapping columns names to columns indexes.
        private readonly Dictionary<string, int> _columnName2columnIndex;

        bool ICollection<CsvDataCollection<TCsvData>>.IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets the <see cref="CsvDataCollection{TCsvData}"/> at the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <returns><see cref="CsvDataCollection{TCsvData}"/> at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/> is less than 0 or greater than <see cref="CsvDataRow.MaxWidth"/>.</exception>
        public CsvDataCollection<TCsvData> this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0 || columnIndex > MaxWidth)
                    throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be non-negative and less than or equal to MaxWidth.");

                return _columns[columnIndex];
            }
        }

        /// <summary>
        /// Gets a <see cref="CsvDataCollection{TCsvData}"/> with the specified column name.
        /// </summary>
        /// <param name="columnName">Column name to look for.</param>
        /// <returns>Instance of <see cref="CsvDataCollection{TCsvData}"/> with the specified column name.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="columnName"/> is null.</exception>
        public CsvDataCollection<TCsvData> this[string columnName]
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
        internal override int KindID
        {
            get
            {
                return _kindId;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="CsvDataCollection{TCsvData}"/> in the <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _columns.Count;
            }
        }
        #endregion

        #region Methods
        internal override void ProxyAdd(object row)
        {
            Add((CsvDataCollection<TCsvData>)row);
        }

        internal override object[] GetAllColumns()
        {
            return _columns.ToArray();
        }

        internal override CsvDataCollection GetByIndex(int index)
        {
            return this[index];
        }

        #region ICollection
        /// <summary>
        /// Adds a <see cref="CsvDataCollection{TCsvData}"/> to the end of the <see cref="CsvDataRow{TCsvData}"/> and updates
        /// the <see cref="CsvDataCollection{TCsvData}"/>'s ID.
        /// </summary>
        /// <param name="dataCollection"><see cref="CsvDataCollection{TCsvData}"/> to add to the <see cref="CsvDataRow{TCsvData}"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dataCollection"/> is null.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="dataCollection"/> is already in another <see cref="CsvDataRow{TCsvData}"/>.
        /// </exception>
        public void Add(CsvDataCollection<TCsvData> dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException("dataCollection");
            if (dataCollection._ref != null)
                throw new ArgumentException("dataCollection is already in another CsvDataRow.", "dataCollection");

            // Can have duplicate column names.
            // E.g: obstacles.csv => Large Stone.
            if (!_columnName2columnIndex.ContainsKey(dataCollection.Name))
                _columnName2columnIndex.Add(dataCollection.Name, _columns.Count);
            else
                Debug.WriteLine("Duplicate column names added: {0}", args: dataCollection.Name);

            // Set index of dataCollection to last index in the row,
            // that way the dataCollection has a correct ID.
            dataCollection._ref = new CsvDataCollectionRef<TCsvData>(_kindId, _columns.Count);
            _columns.Add(dataCollection);
        }

        /// <summary>
        /// Removes the specified <see cref="CsvDataCollection{TCsvData}"/> using its <see cref="CsvDataCollection{TCsvData}.ID"/>.
        /// </summary>
        /// <param name="dataCollection"><see cref="CsvDataCollection{TCsvData}"/> which will be removed.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataCollection"/> is null.</exception>
        public bool Remove(CsvDataCollection<TCsvData> dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException("dataCollection");

            var dataRef = dataCollection._ref;
            if (dataRef == null)
                return false;

            var columnIndex = dataRef.ColumnIndex;
            return RemoveInternal(columnIndex);
        }

        /// <summary>
        /// Removes a <see cref="CsvDataCollection{TCsvData}"/> at the specified column index.
        /// </summary>
        /// <param name="columnIndex">Index of the column to remove.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/> is negative or greater than <see cref="CsvDataRow.MaxWidth"/>.</exception>
        public bool Remove(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > MaxWidth)
                throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be non-negative and less than or equal to MaxWidth.");

            return RemoveInternal(columnIndex);
        }

        /// <summary>
        /// Removes a <see cref="CsvDataCollection{TCsvData}"/> with the specified name.
        /// </summary>
        /// <param name="columnName">Name of the <see cref="CsvDataCollection{TCsvData}"/> to remove.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="columnName"/> is null.</exception>
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
        /// Removes all <see cref="CsvDataCollection{TCsvData}"/> in the <see cref="CsvDataRow{TCsvData}"/>.
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

        /// <summary>
        /// Determines if the <see cref="CsvDataRow{TCsvData}"/> contains the specified <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="dataCollection"><see cref="CsvDataCollection{TCsvData}"/> to check.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataCollection"/> is null.</exception>
        public bool Contains(CsvDataCollection<TCsvData> dataCollection)
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

        /// <summary>
        /// Determines if the <see cref="CsvDataRow{TCsvData}"/> contains a <see cref="CsvDataCollection{TCsvData}"/> at the specified
        /// column index.
        /// </summary>
        /// <param name="columnIndex">Index to check.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/> is negative or greater than <see cref="CsvDataRow.MaxWidth"/>.</exception>
        public bool Contains(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > MaxWidth)
                throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be non-negative and less than or equal to MaxWidth.");

            if (columnIndex > _columns.Count - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Determines if the <see cref="CsvDataRow{TCsvData}"/> contains a <see cref="CsvDataCollection{TCsvData}"/> with the specified
        /// column name.
        /// </summary>
        /// <param name="columnName">Column name to check.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="columnName"/> is null.</exception>
        public bool Contains(string columnName)
        {
            if (columnName == null)
                throw new ArgumentNullException("columnName");

            return _columnName2columnIndex.ContainsKey(columnName);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataRow"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CsvDataCollection<TCsvData>> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        void ICollection<CsvDataCollection<TCsvData>>.CopyTo(CsvDataCollection<TCsvData>[] array, int arrayIndex)
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
