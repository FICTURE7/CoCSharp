using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataRow{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    public class CsvDataRowCollection<TCsvData> : CsvDataRowCollection, ICollection<CsvDataRow<TCsvData>> where TCsvData : CsvData, new()
    {
        #region Constructors
        internal CsvDataRowCollection(CsvDataTable<TCsvData> table) : base(typeof(TCsvData), table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _rows = new List<CsvDataRow<TCsvData>>(48);
            _name2index = new Dictionary<string, int>(48);
        }
        #endregion

        #region Fields & Properties
        private readonly List<CsvDataRow<TCsvData>> _rows;
        private readonly Dictionary<string, int> _name2index;

        bool ICollection<CsvDataRow<TCsvData>>.IsReadOnly => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new CsvDataRow<TCsvData> this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException();
                if (index >= Count)
                    throw new ArgumentOutOfRangeException();

                return _rows[index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new CsvDataRow<TCsvData> this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                return (CsvDataRow<TCsvData>)GetRowByName(name);
            }
        }

        /// <summary>
        /// Gets the number of <see cref="CsvDataRow{TCsvData}"/> in the <see cref="CsvDataRowCollection{TCsvData}"/>.
        /// </summary>
        public override int Count => _rows.Count;
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public override void Add(CsvDataRow row)
        {
            CheckRow(row);

            var nrow = TryCastCsvDataRow(row);
            InternalInsert(nrow, Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public void Add(CsvDataRow<TCsvData> row)
        {
            CheckRow(row);

            InternalInsert(row, Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public override bool Remove(CsvDataRow row)
        {
            CheckRow(row);

            var nrow = TryCastCsvDataRow(row);
            return InternalRemove(nrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public bool Remove(CsvDataRow<TCsvData> row)
        {
            CheckRow(row);

            return InternalRemove(row);
        }

        private bool InternalRemove(CsvDataRow<TCsvData> row)
        {
            var index = _rows.IndexOf(row);
            if (index == -1)
                return false;

            _rows.RemoveAt(index);
            row._table = null;
            row._ref = null;
            row.UpdateRefs();

            for (; index < _rows.Count; index++)
            {
                _rows[index]._ref = new CsvDataRowRef<TCsvData>(Table.KindID, index);
                _rows[index].UpdateRefs();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public override bool Contains(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (row.Table != Table || row.Ref != null)
                return false;

            var nrow = TryCastCsvDataRow(row);
            return _rows.Contains(nrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public bool Contains(CsvDataRow<TCsvData> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (row.Table != Table || row.Ref != null)
                return false;

            return _rows.Contains(row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        public override void CopyTo(CsvDataRow[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index must be non-negative and less than destination array size.");

            var count = _rows.Count - arrayIndex;
            if (count > array.Length)
                throw new ArgumentException("Number of item to copy was larger than destination array size.");

            for (int i = 0; i < count; i++)
                array[arrayIndex + i] = _rows[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CsvDataRow<TCsvData>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index must be non-negative and less than destination array size.");

            _rows.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            _rows.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        public override void InsertAt(CsvDataRow row, int index)
        {
            CheckRow(row);

            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException("Index must be non-negative and less or equal the number of item in the collection.", nameof(index));

            var nrow = TryCastCsvDataRow(row);
            InternalInsert(nrow, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        public void InsertAt(CsvDataRow<TCsvData> row, int index)
        {
            CheckRow(row);

            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException("Index must be non-negative and less or equal the number of item in the collection.", nameof(index));

            InternalInsert(row, index);
        }

        private void InternalInsert(CsvDataRow<TCsvData> row, int index)
        {
            Debug.Assert(row.Name != null);
            // Add the row name to the dictionary which maps row names to row indexes.
            // And ignore rows with name having duplicates.
            if (!_name2index.ContainsKey(row.Name))
                _name2index.Add(row.Name, index);

            row._ref = new CsvDataRowRef<TCsvData>(Table.KindID, index);
            _rows.Insert(index, row);

            // Update the Ref of CsvDataRow after the index where the insert occurred.
            for (; index < _rows.Count; index++)
            {
                _rows[index]._ref = new CsvDataRowRef<TCsvData>(Table.KindID, index);
                _rows[index].UpdateRefs();
            }
        }

        private void CheckRow(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));
            if (row.Table != Table)
                throw new ArgumentException("Row belongs to another table.", nameof(row));
            if (row.Ref != null)
                throw new ArgumentException("Row already belongs to this table.", nameof(row));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<CsvDataRow> GetEnumerator()
        {
            for (int i = 0; i < _rows.Count; i++)
                yield return _rows[i];
        }

        IEnumerator<CsvDataRow<TCsvData>> IEnumerable<CsvDataRow<TCsvData>>.GetEnumerator() => (IEnumerator<CsvDataRow<TCsvData>>)GetEnumerator();

        internal override CsvDataRow GetRowByIndex(int index) => _rows[index];

        internal override CsvDataRow GetRowByName(string name)
        {
            var index = 0;
            if (_name2index.TryGetValue(name, out index))
                return _rows[index];

            return null;
        }

        // Tries to cast specified CsvDataRow to CsvDataRow<TCsvData> if it fails,
        // it throws an ArgumentException.
        private static CsvDataRow<TCsvData> TryCastCsvDataRow(CsvDataRow row)
        {
            var nrow = row as CsvDataRow<TCsvData>;
            if (nrow == null)
                throw new ArgumentException("Unable to explicitly cast row to " + typeof(CsvDataRow<TCsvData>) + ".", nameof(row));

            return nrow;
        }
        #endregion
    }
}
