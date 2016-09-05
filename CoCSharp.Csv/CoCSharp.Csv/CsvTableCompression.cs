namespace CoCSharp.Csv
{
    /// <summary>
    /// Defines the type of compression of .csv files.
    /// </summary>
    public enum CsvTableCompression
    {
        /// <summary>
        /// Table is Compressed.
        /// </summary>
        Compressed = 0,

        /// <summary>
        /// Table is uncompressed.
        /// </summary>
        Uncompressed = 1,

        /// <summary>
        /// Use auto detection of compression. Auto detection of compression is not guaranteed to work.
        /// </summary>
        Auto = 2
    }
}
