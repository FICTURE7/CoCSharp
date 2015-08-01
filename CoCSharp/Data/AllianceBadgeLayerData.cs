namespace CoCSharp.Data
{
    public class AllianceBadgeLayerData : CoCData
    {
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
