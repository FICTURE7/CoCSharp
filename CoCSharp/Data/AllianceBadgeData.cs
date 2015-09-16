namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from alliance_badge.csv.
    /// </summary>
    public class AllianceBadgeData : CoCData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceBadgeData"/> class.
        /// </summary>
        public AllianceBadgeData()
        {
            // Space
        }

        public string Name { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string IconLayer0 { get; set; }
        public string IconLayer1 { get; set; }
        public string IconLayer2 { get; set; }
    }
}
