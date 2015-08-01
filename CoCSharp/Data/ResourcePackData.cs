namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourcePackData : CoCData
    {
        /// <summary>
        /// 
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
