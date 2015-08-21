namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from resources.csv.
    /// </summary>
    public class ResourceData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ResourceData"/> class.
        /// </summary>
        public ResourceData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string SWF { get; set; }
        public string CollectEffect { get; set; }
        public string ResourceIconExportName { get; set; }
        public string StealEffect { get; set; }
        public bool PremiumCurrency { get; set; }
        public string HudInstanceName { get; set; }
        public string CapFullTID { get; set; }
        public int TextRed { get; set; }
        public int TextGreen { get; set; }
        public int TextBlue { get; set; }
        public string WarRefResource { get; set; }
    }
}
