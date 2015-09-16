using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class Obstacle : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with
        /// the specified class.
        /// </summary>
        /// <param name="id">Sets the ID of this <see cref="Obstacle"/> class.</param>
        public Obstacle(int id, Village village)
            : base(id, village)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("loot_multiply_ver")]
        public int LootMultiplier { get; set; }
    }
}
