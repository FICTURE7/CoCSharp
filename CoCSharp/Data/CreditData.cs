using CoCSharp.Data.Csv;

namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from credits.csv.
    /// </summary>
    public class CreditData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CreditData"/> class.
        /// </summary>
        public CreditData()
        {
            // Space
        }

        public string Name { get; set; }
        [CsvProperty("-")]
        public int _ { get; set; } // unknown field
    }
}
