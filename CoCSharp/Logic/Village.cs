using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans village.
    /// </summary>
    public class Village
    {
        /// <summary>
        /// Represents the width of the <see cref="Village"/> layout.
        /// </summary>
        public const int Width = 43;

        /// <summary>
        /// Represents the height of the <see cref="Village"/> layout.
        /// </summary>
        public const int Height = 43;

        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class.
        /// </summary>
        public Village()
        {
            Buildings = new List<Building>();
            Obstacles = new List<Obstacle>();
            Traps = new List<Trap>();
            Decorations = new List<Decoration>();
        }

        /// <summary>
        /// Gets or sets the list of <see cref="Building"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("buildings")]
        public List<Building> Buildings { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Obstacle"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Trap"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("traps")]
        public List<Trap> Traps { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Decoration"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("decos")]
        public List<Decoration> Decorations { get; set; }

        /// <summary>
        /// Gets or sets the experience version.
        /// </summary>
        [JsonProperty("exp_ver")]
        public int ExperienceVersion { get; set; } // 8.x.x clients needs it when there is "loot_multi_ver" stuff

        /// <summary>
        /// Gets or sets the JSON from which the <see cref="Village"/> was
        /// deserialized. Returns <c>null</c> if the <see cref="Village"/> wasn't deserialized.
        /// </summary>
        [JsonIgnore]
        public string DeserializedJson { get; set; }

        /// <summary>
        /// Returns a JSON string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A JSON string that represents the current <see cref="Village"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON string and indented if specified that represents the current <see cref="Village"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON string and indented if specified that represents the current <see cref="Village"/>.</returns>
        public string ToJson(bool indent)
        {
            return indent == true ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        public static Village FromJson(string value)
        {
            var village = JsonConvert.DeserializeObject<Village>(value);
            village.DeserializedJson = value;
            return village;
        }
    }
}
