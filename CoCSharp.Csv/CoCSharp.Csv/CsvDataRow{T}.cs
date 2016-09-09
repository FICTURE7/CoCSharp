using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
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

        bool ICollection<CsvDataCollection<TCsvData>>.IsReadOnly => false;

        /// <summary>
        /// Gets the <see cref="CsvDataCollection{TCsvData}"/> at the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <returns><see cref="CsvDataCollection{TCsvData}"/> at the specified index.</returns>
        public CsvDataCollection<TCsvData> this[int columnIndex]
        {
            get
            {
                if (!ContainsInternal(columnIndex))
                    return null;

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
                    throw new ArgumentNullException(nameof(columnName));

                // Look for a column index with the specified column name in the dictionary.
                var columnIndex = 0;
                if (!_columnName2columnIndex.TryGetValue(columnName, out columnIndex))
                    return null;

                return _columns[columnIndex];
            }
        }

        // Kind ID of TCsvData, also known as the row index.
        private readonly int _kindId;
        internal override int KindID => _kindId;

        /// <summary>
        /// Gets the number of <see cref="CsvDataCollection{TCsvData}"/> in the <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        public override int Count => _columns.Count;
        #endregion

        #region Methods
        internal override CsvDataCollection GetByIndex(int index)
        {
            return this[index];
        }

        #region ICollection
        /// <summary>
        /// Determines if the <see cref="CsvDataRow{TCsvData}"/> contains the specified <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="dataCollection"><see cref="CsvDataCollection{TCsvData}"/> to check.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataCollection"/> is null.</exception>
        public bool Contains(CsvDataCollection<TCsvData> dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));

            var columnIndex = dataCollection.CollectionRef.ColumnIndex;
            return ContainsInternal(columnIndex);
        }

        /// <summary>
        /// Determines if the <see cref="CsvDataRow{TCsvData}"/> contains a <see cref="CsvDataCollection{TCsvData}"/> at the specified
        /// column index.
        /// </summary>
        /// <param name="columnIndex">Index to check.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/> is greater than <see cref="CsvDataRow.MaxWidth"/>.</exception>
        public bool Contains(int columnIndex)
        {
            if (columnIndex > MaxWidth)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index must be less than CsvDataRow.MaxWidth.");

            return ContainsInternal(columnIndex);
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
                throw new ArgumentNullException(nameof(columnName));

            return _columnName2columnIndex.ContainsKey(columnName);
        }

        public override bool Contains(CsvDataCollection dataCollection)
        {
            return Contains((CsvDataCollection<TCsvData>)dataCollection);
        }

        private bool ContainsInternal(int columnIndex) => !(columnIndex < 0 || columnIndex > Count - 1);

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
            if (Count == MaxWidth)
                throw new InvalidOperationException("Reached the capacity of CsvDataRow which is MaxWidth.");
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));
            if (dataCollection.CollectionRef != CsvDataCollectionRef<TCsvData>.NullRef)
                throw new ArgumentException("CsvDataCollection is already in a CsvDataRow.", nameof(dataCollection));

            InsertInternal(_columns.Count, dataCollection);
        }

        public override void Add(CsvDataCollection dataCollection)
        {
            if (Count == MaxWidth)
                throw new InvalidOperationException("Reached the capacity of CsvDataRow which is MaxWidth.");
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));
            if (dataCollection.CollectionRef != CsvDataCollectionRef<TCsvData>.NullRef)
                throw new ArgumentException("CsvDataCollection is already in a CsvDataRow.", nameof(dataCollection));

            InsertInternal(_columns.Count, (CsvDataCollection<TCsvData>)dataCollection);
        }

        public void Insert(int index, CsvDataCollection<TCsvData> dataCollection)
        {
            if (Count == MaxWidth)
                throw new InvalidOperationException("Reached the capacity of CsvDataRow which is MaxWidth.");
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));
            if (dataCollection.CollectionRef != null)
                throw new ArgumentException("CsvDataCollection is already in a CsvDataRow.", nameof(dataCollection));

            InsertInternal(index, dataCollection);
        }

        public override void Insert(int index, CsvDataCollection dataCollection)
        {
            Insert(index, (CsvDataCollection<TCsvData>)dataCollection);
        }

        private void InsertInternal(int index, CsvDataCollection<TCsvData> dataCollection)
        {
            Debug.Assert(index <= MaxWidth);
            // Can have duplicate column names.
            // E.g: obstacles.csv => Large Stone.
            if (!_columnName2columnIndex.ContainsKey(dataCollection.Name))
                _columnName2columnIndex.Add(dataCollection.Name, _columns.Count);
            else
                Debug.WriteLine("Duplicate column names added: '{0}'", args: dataCollection.Name);

            // Set index of dataCollection to last index in the row,
            // that way the dataCollection has a correct ID.
            var newRef = new CsvDataCollectionRef<TCsvData>(_kindId, index);
            dataCollection._ref = newRef;

            // Updates the old CsvData's ref in the dataCollection.
            // So we have the ref up to date.
            for (int i = 0; i < dataCollection.Count; i++)
                dataCollection[i]._ref = newRef;

            _columns.Insert(index, dataCollection);
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
                throw new ArgumentNullException(nameof(dataCollection));

            var columnIndex = dataCollection.CollectionRef.ColumnIndex;
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
                return false;

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
                throw new ArgumentNullException(nameof(columnName));

            var columnIndex = default(int);
            if (!_columnName2columnIndex.TryGetValue(columnName, out columnIndex))
                return false;

            return RemoveInternal(columnIndex);
        }

        public override bool Remove(CsvDataCollection dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));

            var columnIndex = dataCollection.CollectionRef.ColumnIndex;
            return RemoveInternal(columnIndex);
        }

        private bool RemoveInternal(int columnIndex)
        {
            if (columnIndex > _columns.Count - 1)
                return false;

            var column = _columns[columnIndex];
            // Make sure _ref points to NullRef, to mark it
            // as its not a in a CsvDataRow.
            column._ref = CsvDataCollectionRef<TCsvData>.NullRef;

            _columnName2columnIndex.Remove(column.Name);
            _columns.RemoveAt(columnIndex);
            return true;
        }

        /// <summary>
        /// Removes all <see cref="CsvDataCollection{TCsvData}"/> in the <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                var column = _columns[i];
                column._ref = CsvDataCollectionRef<TCsvData>.NullRef;
            }
            _columns.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataRow"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="CsvDataRow"/>.</returns>
        public override IEnumerator<CsvDataCollection> GetEnumerator() => _columns.GetEnumerator();

        IEnumerator<CsvDataCollection<TCsvData>> IEnumerable<CsvDataCollection<TCsvData>>.GetEnumerator() => _columns.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _columns.GetEnumerator();

        void ICollection<CsvDataCollection<TCsvData>>.CopyTo(CsvDataCollection<TCsvData>[] array, int arrayIndex)
        {
            _columns.CopyTo(array, arrayIndex);
        }

        public override void CopyTo(CsvDataCollection[] array, int arrayIndex)
        {
            // Not the most efficient way, but it works.
            Array.Copy(_columns.ToArray(), arrayIndex, array, arrayIndex, _columns.Count - arrayIndex);
        }
        #endregion
        #endregion
    }
}
