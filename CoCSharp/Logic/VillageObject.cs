using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an object in a <see cref="Village"/>.
    /// </summary>
    public abstract class VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VillageObject"/> class.
        /// </summary>
        public VillageObject()
        {
            // Space
        }

        /// <summary>
        /// Initailizes a new instance of the <see cref="VillageObject"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="VillageObject"/>.</param>
        public VillageObject(int dataID)
        {
            DataID = dataID;
        }

        /// <summary>
        /// Gets or sets the data ID of the <see cref="VillageObject"/>.
        /// </summary>
        [JsonProperty("data")]
        public int DataID { get; set; } //TODO: Hide this thing

        /// <summary>
        /// Gets or sets the X coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}
