namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from alliance_portal.csv.
    /// </summary>
    public class AlliancePortalData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="AlliancePortalData"/> class.
        /// </summary>
        public AlliancePortalData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
