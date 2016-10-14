namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a column in a <see cref="CsvDataTable"/>.
    /// </summary>
    public class CsvDataColumn
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataColumn"/> class.
        /// </summary>
        public CsvDataColumn()
        {
            _dataLevel = -1;
        }
        #endregion

        #region Fields & Properties
        internal int _dataLevel;
        internal CsvDataTable _table;

        /// <summary>
        /// Gets the <see cref="CsvDataTable"/> which contains this <see cref="CsvDataColumn"/>.
        /// </summary>
        public CsvDataTable Table => _table;

        /// <summary>
        /// Gets the level of the data stored by the <see cref="CsvDataColumn"/>.
        /// </summary>
        public int DataLevel => _dataLevel;
        #endregion
    }
}
