namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionData : CoCData
    {
        /// <summary>
        /// 
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
