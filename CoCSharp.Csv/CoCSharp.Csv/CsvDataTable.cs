using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a set of <see cref="CsvDataRow"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvDataTable : ICollection<CsvDataRow>
    {
        // The table will have the following form.
        // Where the row index is the type of CsvData,
        // E.g: 1000001 will point to row 1 which is of type BuildingData.
        //    : 4000012 will point to row 4 which is of type CharacterData.
        // and where the column index points to the data array.
        // +---+----------+------------------+----------------+----------------+-------------------+----------+
        // |   |          | 0                | 1              | 2              | 3                 | ..999999 |
        // +---+----------+------------------+----------------+----------------+-------------------+----------+
        // | 0 |          |  "Troop Housing" |  "Town Hall"   |  "Elixir Pump" |  "Elixir Storage" |          |
        // +---+----------+------------------+----------------+----------------+-------------------+----------+
        // | 1 | Building |  BuildingData[]  | BuildingData[] |  BuildinData[] |   BuildingData[]  |          |
        // +---+----------+------------------+----------------+----------------+-------------------+----------+
        // | 2 |  Locale  |   LocaleData[]   |  LocaleData[]  |  LocaleData[]  |    LocaleData[]   |          |
        // +---+----------+------------------+----------------+----------------+-------------------+----------+

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataTable"/> class.
        /// </summary>
        public CsvDataTable()
        {
            _rows = new CsvDataRow[32];
        }
        #endregion

        #region Fields & Properties
        bool ICollection<CsvDataRow>.IsReadOnly => false;

        private CsvDataRow[] _rows;
        /// <summary>
        /// Gets the <see cref="CsvDataRow"/> at the specified row index.
        /// </summary>
        /// <param name="rowIndex">Zero-based index of the <see cref="CsvDataRow"/> to locate.</param>
        /// <returns>Instance of the <see cref="CsvDataRow"/> if found; otherwise, null.</returns>
        public CsvDataRow this[int rowIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex > Count)
                    throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index must be non-negative and less than Count.");

                return _rows[rowIndex];
            }
        }

        private int _count;
        /// <summary>
        /// Gets the number of <see cref="CsvDataRow"/> in the <see cref="CsvDataTable"/>.
        /// </summary>
        public int Count => _count;
        #endregion

        #region Methods
        #region ICollection
        public void Add(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            AddInternal(row);
        }

        bool ICollection<CsvDataRow>.Remove(CsvDataRow item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CsvDataRow>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<CsvDataRow>.Contains(CsvDataRow item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CsvDataRow>.CopyTo(CsvDataRow[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<CsvDataRow> IEnumerable<CsvDataRow>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Returns the <see cref="CsvDataRow{TCsvData}"/> for the specified type.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
        /// <returns><see cref="CsvDataRow{TCsvData}"/> for the specified type; returns null if not loaded.</returns>
        public CsvDataRow<TCsvData> GetRow<TCsvData>() where TCsvData : CsvData, new()
        {
            var type = typeof(TCsvData);
            return (CsvDataRow<TCsvData>)GetRowInternal(type);
        }

        /// <summary>
        /// Returns the <see cref="CsvDataRow"/> for the specified type.
        /// </summary>
        /// <param name="type">Type of <see cref="CsvData"/>.</param>
        /// <returns><see cref="CsvDataRow"/> for the specified type; returns null if not loaded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="type"/> is abstract or is does not inherit from <see cref="CsvData"/>.</exception>
        public CsvDataRow GetRow(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsAbstract || type.BaseType != typeof(CsvData))
                throw new ArgumentException("type must be inherited from CsvData and non-abstract.", "type");

            return GetRowInternal(type);
        }

        private CsvDataRow GetRowInternal(Type type)
        {
            var kindId = CsvData.GetInstance(type).KindID;
            return _rows[kindId];
        }

        //private static int GetRowIndex(int id)
        //{
        //    return id / InternalConstants.IDBase;
        //}

        //private static int GetColumnIndex(int id, int rowIndex)
        //{
        //    return id - (rowIndex * InternalConstants.IDBase);
        //}

        private void AddInternal(CsvDataRow item)
        {
            var index = item.KindID;
            var arrayLength = _rows.Length - 1;
            if (index > arrayLength)
                Array.Resize(ref _rows, index + 4);

            _rows[index] = item;
            _count++;
        }

        private bool RemoveInternal(int kindId)
        {
            var index = kindId;
            if (index > _rows.Length - 1)
                return false;

            return true;
        }
    }
    #endregion
}
