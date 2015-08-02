namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from alliance_badge_layers.csv.
    /// </summary>
    public class AllianceBadgeLayerData : CoCData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceBadgeLayerData"/> class.
        /// </summary>
        public AllianceBadgeLayerData()
        {
            // Space
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public int RequiredClanLevel { get; set; }
    }
}
