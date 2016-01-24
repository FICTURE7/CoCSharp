using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file data.
    /// </summary>
    public abstract class CsvData
    {
        /// <summary>
        /// Gets or sets the data index of the <see cref="CsvData"/>.
        /// </summary>
        public int DataIndex { get; set; }

        /// <summary>
        /// Gets or sets the data ID of the <see cref="CsvData"/>.
        /// </summary>
        [CsvIgnore]
        public int DataID { get { return DataIndex + BaseDataID; } }

        /// <summary>
        /// Gets the base data ID of the <see cref="CsvData"/>.
        /// </summary>
        public abstract int BaseDataID { get; }
    }
}
