namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DecorationData : CoCData
    {
        /// <summary>
        /// 
        /// </summary>
        public DecorationData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public string ExportNameConstruction { get; set; }
        public string BuildResource { get; set; }
        public int BuildCost { get; set; }
        public int RequiredExpLevel { get; set; }
        public int MaxCount { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Icon { get; set; }
        public string ExportNameBase { get; set; }
        public string ExportNameBaseNpc { get; set; }
        public string ExportNameBaseWar { get; set; }
    }
}
