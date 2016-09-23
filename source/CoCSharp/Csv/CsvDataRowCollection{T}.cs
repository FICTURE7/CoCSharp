using System;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataRow{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData"></typeparam>
    public class CsvDataRowCollection<TCsvData> : CsvDataRowCollection, ICollection<CsvDataRow<TCsvData>> where TCsvData : CsvData, new()
    {
        #region Constructors
        internal CsvDataRowCollection(CsvDataTable<TCsvData> table) : base(typeof(TCsvData), table)
        {
            _rows = new List<CsvDataRow<TCsvData>>();
        }
        #endregion

        #region Fields & Properties
        private readonly List<CsvDataRow<TCsvData>> _rows;

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
                    throw new IndexOutOfRangeException();
                if (index > Count - 1)
                    throw new IndexOutOfRangeException();

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

                return (CsvDataRow<TCsvData>)GetFromName(name);
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
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (row.Table != null)
                throw new ArgumentException("The row already belongs to this or another table.");

            var nrow = TryCastCsvDataRow(row);
            _rows.Add(nrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public void Add(CsvDataRow<TCsvData> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _rows.Add(row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public override bool Remove(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var nrow = TryCastCsvDataRow(row);
            return _rows.Remove(nrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public bool Remove(CsvDataRow<TCsvData> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return _rows.Remove(row);
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

            return _rows.Contains(row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public override void CopyTo(CsvDataRow[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CsvDataRow<TCsvData>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
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
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        public void InsertAt(CsvDataRow<TCsvData> row, int index)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));
            if (row.Table != Table)
                throw new ArgumentException("Row belongs to another table.");
            if (row.Ref != null)
                throw new ArgumentException("Row already belongs to this table.");
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException();

            row._ref = new CsvDataRowRef<TCsvData>(Table.KindID, index);
            _rows.Insert(index, row);

            // Update the Ref of CsvDataRow after the index where the insert occurred.
            for (; index < _rows.Count; index++)
                _rows[index]._ref = new CsvDataRowRef<TCsvData>(Table.KindID, index);
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

        internal override CsvDataRow GetAtIndex(int index) => _rows[index];

        internal override CsvDataRow GetFromName(string name)
        {
            throw new NotImplementedException();
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
