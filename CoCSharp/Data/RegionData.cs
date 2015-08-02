namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from regions.csv.
    /// </summary>
    public class RegionData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="RegionData"/> class.
        /// </summary>
        public RegionData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string DisplayName { get; set; }
        public bool IsCountry { get; set; }
    }
}
