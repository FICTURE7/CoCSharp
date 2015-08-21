namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from hints.csv.
    /// </summary>
    public class HintData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="HintData"/> class.
        /// </summary>
        public HintData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public int TownHallLevelMin { get; set; }
        public int TownHallLevelMax { get; set; }
        public string iOSTID { get; set; }
        public string AndroidTID { get; set; }
    }
}
