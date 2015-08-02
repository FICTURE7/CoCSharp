namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from resource_packs.csv.
    /// </summary>
    public class ResourcePackData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="ResourcePackData"/> class.
        /// </summary>
        public ResourcePackData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string Resource { get; set; }
        public int CapacityPercentage { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
    }
}
