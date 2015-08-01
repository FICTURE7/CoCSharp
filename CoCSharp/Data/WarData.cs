namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class WarData : CoCData
    {
        /// <summary>
        /// 
        /// </summary>
        public WarData()
        {
            // Space
        }
        
        public string Name { get; set; }
        public int TeamSize { get; set; }
        public int PreparationMinutes { get; set; }
        public int WarMinutes { get; set; }
        public bool DisableProduction { get; set; }
    }
}
