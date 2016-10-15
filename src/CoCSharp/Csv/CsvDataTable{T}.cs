namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a strongly typed table of <see cref="CsvData"/> of the specified type, <typeparamref name="TCsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to store.</typeparam>
    public class CsvDataTable<TCsvData> : CsvDataTable where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataTable{TCsvData}"/> class.
        /// </summary>
        public CsvDataTable() : base(typeof(TCsvData))
        {
            _rows = (CsvDataRowCollection<TCsvData>)base.Rows;
        }
        #endregion

        #region Fields & Properties
        private readonly CsvDataRowCollection<TCsvData> _rows;

        /// <summary>
        /// Gets the <see cref="CsvDataRowCollection{TCsvData}"/> associated with this <see cref="CsvDataTable{TCsvData}"/>.
        /// </summary>
        public new CsvDataRowCollection<TCsvData> Rows => _rows;
        #endregion

        #region Methods
        #endregion
    }
}
