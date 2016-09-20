using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a reference to a <see cref="CsvDataRow"/>.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, Column = {ColumnIndex}, Row = {RowIndex}")]
    public class CsvDataRowRef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataRowRef"/> class with the specified ID.
        /// </summary>
        /// <param name="id">ID from which the row index and column index will be calculated.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than 0 or is less than 1000000.</exception>
        public CsvDataRowRef(int id)
        {
            if (id < 0 || id < InternalConstants.IDBase)
                throw new ArgumentOutOfRangeException("id", "id must be non-negative and greater or equal to 1000000.");

            _id = id;
            _rowIndex = id / InternalConstants.IDBase;
            _columnIndex = id - (_rowIndex * InternalConstants.IDBase);
        }

        public CsvDataRowRef(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || columnIndex < 0)
                throw new ArgumentOutOfRangeException("rowIndex, columnIndex", "rowIndex and columnIndex must be non-negative.");
            if (columnIndex > CsvDataTable.MaxWidth)
                throw new ArgumentOutOfRangeException("columnIndex", "columnIndex must be less or equal to CsvDataRow.MaxWidth.");

            _rowIndex = rowIndex;
            _columnIndex = ColumnIndex;
            _id = rowIndex * InternalConstants.IDBase + columnIndex;
        }

        private readonly int _id;
        /// <summary>
        /// Gets the ID of the <see cref="CsvDataRowRef"/> class.
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
        /// Gets the row index of the <see cref="CsvDataRowRef"/> class.
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
        /// Gets the column index of the <see cref="CsvDataRowRef"/> class.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return _columnIndex;
            }
        }

        /// <summary>
        /// Returns the <see cref="CsvDataRow"/> which this <see cref="CsvDataRowRef"/> is pointing to
        /// from the specified <see cref="CsvDataTableCollection"/>.
        /// </summary>
        /// <param name="table"><see cref="CsvDataTableCollection"/> from which to retrieve the <see cref="CsvDataRow"/>.</param>
        /// <returns>
        /// Returns the instance of <see cref="CsvDataRow"/> which this <see cref="CsvDataRowRef"/> is pointing to.
        /// If not found, null will be returned.
        /// </returns>
        public CsvDataRow Get(CsvDataTableCollection table)
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
    /// Represents a reference to a <see cref="CsvDataRow{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    public class CsvDataRowRef<TCsvData> : CsvDataRowRef where TCsvData : CsvData, new()
    {
        public CsvDataRowRef(int id) : base(id)
        {
            // Space
        }

        public CsvDataRowRef(int rowIndex, int columnIndex) : base(rowIndex, columnIndex)
        {
            // Space
        }

        public CsvDataRow<TCsvData> Get(CsvDataTableCollection table)
        {
            return (CsvDataRow<TCsvData>)base.Get(table);
        }
    }
}
