using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans building.
    /// </summary>
    public class Building : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        public Building() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Building"/>.</param>
        public Building(int dataID) : base(dataID)
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the level of the <see cref="Building"/>.
        /// </summary>
        [JsonProperty("lvl")]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets whether the <see cref="Building"/> is locked.
        /// This is for mostly for the alliance castle building.
        /// </summary>
        [JsonProperty("locked", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Locked { get; set; }
    }
}
