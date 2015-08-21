using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class Building : VillageObject
    {
        /// <summary>
        /// Initailizes a new instance of the <see cref="Building"/> class with the specified
        /// ID and level.
        /// </summary>
        /// <param name="id">Sets the ID of this <see cref="Building"/> class.</param>
        /// <param name="level">Sets the Level of this <see cref="Building"/> class</param>
        public Building(int id, int level, Village village)
            : base(id, village)
        {
            Level = level;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("hp")] // hitpoints
        public int Hitpoints { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lvl")]
        public int Level { get; set; }
    }
}
