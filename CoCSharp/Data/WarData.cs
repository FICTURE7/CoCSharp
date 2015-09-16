namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from war.csv.
    /// </summary>
    public class WarData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="WarData"/> class.
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
