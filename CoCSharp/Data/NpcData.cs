namespace CoCSharp.Data
{
    /// <summary>
    ///  Defines data from npc.csv.
    /// </summary>
    public class NpcData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="NpcData"/> class.
        /// </summary>
        public NpcData()
        {
            // Space
        }

        public string Name { get; set; }
        public string MapInstanceName { get; set; }
        public string MapDependencies { get; set; }
        public string TID { get; set; }
        public int ExpLevel { get; set; }
        public string UnitType { get; set; }
        public int UnitCount { get; set; }
        public string LevelFile { get; set; }
        public int Gold { get; set; }
        public int Elixir { get; set; }
        public bool AlwaysUnlocked { get; set; }
    }
}
