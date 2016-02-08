using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans trap.
    /// </summary>
    public class Trap : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class.
        /// </summary>
        public Trap() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Trap"/>.</param>
        public Trap(int dataID) : base(dataID)
        {
            // Space
        }

        internal override int BaseDataID { get { return 12000000; } }

        internal override int BaseGameID { get { return 504000000; } }

        /// <summary>
        /// Gets or sets whether the trap needs to be repaired.
        /// </summary>
        [JsonProperty("need_repair", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Broken { get; set; }
    }
}
