using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans obstacle.
    /// </summary>
    public class Obstacle : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class.
        /// </summary>
        public Obstacle() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Obstacle"/>.</param>
        public Obstacle(int dataID) : base(dataID)
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the loot multiplier of the <see cref="Obstacle"/>.
        /// </summary>
        [JsonProperty("loot_multiply_ver", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LootMultiplier { get; set; }
    }
}
