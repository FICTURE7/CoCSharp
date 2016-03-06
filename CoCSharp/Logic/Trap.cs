using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans trap.
    /// </summary>
    public class Trap : VillageObject
    {
        internal const int BaseGameID = 504000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class.
        /// </summary>
        public Trap() : base()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets whether the trap needs to be repaired.
        /// </summary>
        [JsonProperty("need_repair", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Broken { get; set; }
    }
}
