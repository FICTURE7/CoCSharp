using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a reference to a <see cref="CsvDataCollection"/>.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, Column = {ColumnIndex}, Row = {RowIndex}")]
    public class CsvDataCollectionRef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataCollectionRef"/> class with the specified ID.
        /// </summary>
        /// <param name="id">ID from which the row index and column index will be calculated.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than 1000000.</exception>
        public CsvDataCollectionRef(int id)
        {
            if (id < InternalConstants.IDBase)
                throw new ArgumentOutOfRangeException("id", "id must be greater or equal to 1000000.");

            _id = id;
            _rowIndex = id / InternalConstants.IDBase;
            _columnIndex = id - (_rowIndex * InternalConstants.IDBase);
        }

        public CsvDataCollectionRef(int rowIndex, int columnIndex)
        {
            if (rowIndex < 1)
                throw new ArgumentOutOfRangeException("rowIndex", "rowIndex must be greater or equal to 1 and columnIndex must be non-negative.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException("columnIndex", "rowIndex must be greater or equal to 1 and columnIndex must be non-negative.");
            if (columnIndex > CsvDataRow.MaxWidth)
                throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be less or equal to CsvDataRow.MaxWidth.");

            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _id = (rowIndex * InternalConstants.IDBase) + columnIndex;
        }

        private readonly int _id;
        /// <summary>
        /// Gets the ID of the <see cref="CsvDataCollectionRef"/> class.
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
        }

        private readonly int _rowIndex;
        /// <summary>
        /// Gets the row index of the <see cref="CsvDataCollectionRef"/> class.
        /// </summary>
        public int RowIndex
        {
            get
            {
                return _rowIndex;
            }
        }

        private readonly int _columnIndex;
        /// <summary>
        /// Gets the column index of the <see cref="CsvDataCollectionRef"/> class.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return _columnIndex;
            }
        }

        /// <summary>
        /// Returns the <see cref="CsvDataCollection"/> which this <see cref="CsvDataCollectionRef"/> is pointing to
        /// from the specified <see cref="CsvDataTable"/>.
        /// </summary>
        /// <param name="table"><see cref="CsvDataTable"/> from which to retrieve the <see cref="CsvDataCollection"/>.</param>
        /// <returns>
        /// Returns the instance of <see cref="CsvDataCollection"/> which this <see cref="CsvDataCollectionRef"/> is pointing to.
        /// If not found, null will be returned.
        /// </returns>
        public CsvDataCollection Get(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            var row = table[RowIndex];
            if (row == null)
                return null;
            return row.GetByIndex(ColumnIndex);
        }
    }

    /// <summary>
    /// Represents a reference to a <see cref="CsvDataCollection{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    public class CsvDataCollectionRef<TCsvData> : CsvDataCollectionRef where TCsvData : CsvData, new()
    {
        public CsvDataCollectionRef(int id) : base(id)
        {
            // Space
        }

        public CsvDataCollectionRef(int rowIndex, int columnIndex) : base(rowIndex, columnIndex)
        {
            // Space
        }

        public CsvDataCollection<TCsvData> Get(CsvDataTable table)
        {
            return (CsvDataCollection<TCsvData>)base.Get(table);
        }
    }
}
