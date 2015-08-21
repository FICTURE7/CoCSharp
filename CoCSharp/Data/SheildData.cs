namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from shields.csv.
    /// </summary>
    public class SheildData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="SheildData"/> class.
        /// </summary>
        public SheildData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public int TimeH { get; set; }
        public int Diamonds { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public int CooldownS { get; set; }
        public int LockedAboveScore { get; set; }
    }
}
