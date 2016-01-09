namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file data.
    /// </summary>
    public abstract class CsvData
    {
        /// <summary>
        /// Gets or sets the data ID of the <see cref="CsvData"/>.
        /// </summary>
        public int DataID { get; set; }
    }
}
