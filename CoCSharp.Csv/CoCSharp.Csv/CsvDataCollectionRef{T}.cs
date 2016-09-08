namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a reference to a <see cref="CsvDataCollection{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    public class CsvDataCollectionRef<TCsvData> : CsvDataCollectionRef where TCsvData : CsvData, new()
    {
        public static readonly new CsvDataCollectionRef<TCsvData> NullRef = new CsvDataCollectionRef<TCsvData>();

        protected CsvDataCollectionRef() : base()
        {
            // Space 
        }

        public CsvDataCollectionRef(int id) : base(id)
        {
            // Space
        }

        public CsvDataCollectionRef(int rowIndex, int columnIndex) : base(rowIndex, columnIndex)
        {
            // Space
        }

        public new CsvDataCollection<TCsvData> Get(CsvDataTable table)
        {
            return (CsvDataCollection<TCsvData>)base.Get(table);
        }
    }
}
