namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from experience_levels.csv.
    /// </summary>
    public class LevelData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="LevelData"/> class.
        /// </summary>
        public LevelData()
        {
            // Space
        }

        public string Name { get; set; }
        public int ExpPoints { get; set; }
    }
}
