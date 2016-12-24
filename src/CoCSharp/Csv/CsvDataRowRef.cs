using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a reference to a <see cref="CsvDataRow"/>.
    /// </summary>
    [DebuggerDisplay("ID = {ID}")]
    public class CsvDataRowRef
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataRowRef"/> class with the specified ID.
        /// </summary>
        /// <param name="id">ID from which the row index and column index will be calculated.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than 0 or is less than 1000000.</exception>
        public CsvDataRowRef(int id)
        {
            if (id < 0 || id < InternalConstants.IdBase)
                throw new ArgumentOutOfRangeException("id", "id must be non-negative and greater or equal to 1000000.");

            _id = id;
            _tableIndex = id / InternalConstants.IdBase;
            _rowIndex = id - (_tableIndex * InternalConstants.IdBase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <param name="rowIndex"></param>
        public CsvDataRowRef(int tableIndex, int rowIndex)
        {
            if (tableIndex < 0 || rowIndex < 0)
                throw new ArgumentOutOfRangeException(null, "rowIndex and columnIndex must be non-negative.");
            if (rowIndex > CsvDataTable.MaxHeight)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "columnIndex must be less or equal to CsvDataRow.MaxHeight.");

            _tableIndex = tableIndex;
            _rowIndex = rowIndex;
            _id = (tableIndex * InternalConstants.IdBase) + rowIndex;
        }
        #endregion

        #region Fields & Properties
        private readonly int _id;
        private readonly int _tableIndex;
        private readonly int _rowIndex;

        /// <summary>
        /// Gets the ID of the <see cref="CsvDataRowRef"/> class.
        /// </summary>
        public int Id => _id;

        /// <summary>
        /// Gets the table index of the <see cref="CsvDataRowRef"/> class.
        /// </summary>
        public int TableIndex => _tableIndex;

        /// <summary>
        /// Gets the row index of the <see cref="CsvDataRowRef"/> class.
        /// </summary>
        public int RowIndex => _rowIndex;
        #endregion

        #region Methods
        /// <summary>
        /// Returns the <see cref="CsvDataRow"/> which this <see cref="CsvDataRowRef"/> is pointing to
        /// from the specified <see cref="CsvDataTableCollection"/>.
        /// </summary>
        /// <param name="tableCollection"><see cref="CsvDataTableCollection"/> from which to retrieve the <see cref="CsvDataRow"/>.</param>
        /// <returns>
        /// Returns the instance of <see cref="CsvDataRow"/> which this <see cref="CsvDataRowRef"/> is pointing to.
        /// If not found, null will be returned.
        /// </returns>
        public CsvDataRow Get(CsvDataTableCollection tableCollection)
        {
            if (tableCollection == null)
                throw new ArgumentNullException(nameof(tableCollection));

            var table = tableCollection[TableIndex];
            if (table == null)
                return null;
            return table.Rows[RowIndex];
        }
        #endregion
    }

    /// <summary>
    /// Represents a reference to a <see cref="CsvDataRow{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    public class CsvDataRowRef<TCsvData> : CsvDataRowRef where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public CsvDataRowRef(int id) : base(id)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <param name="rowIndex"></param>
        public CsvDataRowRef(int tableIndex, int rowIndex) : base(tableIndex, rowIndex)
        {
            // Space
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public new CsvDataRow<TCsvData> Get(CsvDataTableCollection table)
        {
            return (CsvDataRow<TCsvData>)base.Get(table);
        }
        #endregion
    }
}
