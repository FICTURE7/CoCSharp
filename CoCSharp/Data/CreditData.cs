using CoCSharp.Data.Csv;

namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class CreditData : CoCData
    {
        /// <summary>
        /// 
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
