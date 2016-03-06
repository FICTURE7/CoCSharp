using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans obstacle.
    /// </summary>
    public class Obstacle : VillageObject
    {
        internal const int BaseGameID = 503000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class.
        /// </summary>
        public Obstacle() : base()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the loot multiplier of the <see cref="Obstacle"/>.
        /// </summary>
        [JsonProperty("loot_multiply_ver", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LootMultiplier { get; set; }

        [JsonProperty("clear_t", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int _clearTime;
        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> of the clear time
        /// of the building.
        /// </summary>
        [JsonIgnore]
        public TimeSpan ClearTime
        {
            get { return TimeSpan.FromSeconds(_clearTime + 100); } //TODO: Check if _clearTime == 0 before adding 100
            set { _clearTime = (int)value.TotalSeconds; }
        }
    }
}
