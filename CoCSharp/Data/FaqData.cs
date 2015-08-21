namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from faq.csv.
    /// </summary>
    public class FaqData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="FaqData"/> class.
        /// </summary>
        public FaqData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
    }
}
