namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file.
    /// </summary>
    public abstract class CsvData
    {
        /// <summary>
        /// Gets the data ID of the <see cref="CsvData"/>.
        /// </summary>
        [CsvIgnore]
        public int ID { get { return Index + BaseDataID; } }

        // Index of the CsvData.
        internal int Index { get; set; }

        // Base data ID, Example: 1000000 for BuildingData
        internal abstract int BaseDataID { get; }
    }
}
