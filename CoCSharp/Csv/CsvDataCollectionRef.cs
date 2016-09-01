using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a reference to a <see cref="CsvDataCollection"/>.
    /// </summary>
    public class CsvDataCollectionRef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataCollectionRef"/> class with the specified ID.
        /// </summary>
        /// <param name="id">ID from which the row index and column index will be calculated.</param>
        public CsvDataCollectionRef(int id)
        {
            _id = id;
            _rowIndex = id / InternalConstants.IDBase;
            _columnIndex = id - (_rowIndex * InternalConstants.IDBase);
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
}
