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
        /// Represents the minimum legal value of the X coordinate. This field is constant.
        /// </summary>
        public const int MinX = 2;
        
        /// <summary>
        /// Represents the minimum legal value of the Y coordinate. This field is constant.
        /// </summary>
        public const int MinY = 2;

        /// <summary>
        /// Represents the maximum legal value of the X coordinate. This field is constant.
        /// </summary>
        public const int MaxX = 38;

        /// <summary>
        /// Represents the maximum legal value of the Y coordinate. This field is constant.
        /// </summary>
        public const int MaxY = 38;

        /// <summary>
        /// Represents the width of the <see cref="Village"/> layout.
        /// </summary>
        public const int Width = 40;

        /// <summary>
        /// Represents the height of the <see cref="Village"/> layout.
        /// </summary>
        public const int Height = 40;

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
        /// Gets or sets the JSON from which the <see cref="Village"/> was
        /// deserialized. Returns <c>null</c> if the <see cref="Village"/> wasn't deserialized.
        /// </summary>
        [JsonIgnore]
        public string DeserializedJson { get; set; }

        /// <summary>
        /// Returns a JSON formatted string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A JSON formatted string that represents the current <see cref="Village"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON formatted string and indented if specified that represents the current <see cref="Village"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON formatted string and indented if specified that represents the current <see cref="Village"/>.</returns>
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

        /// <summary>
        /// Determines if the specified coordinates is legal. Returns <c>true</c>
        /// if its valid.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Returns <c>true</c> if coordinate is valid.</returns>
        public static bool IsCoordinatesLegal(int x, int y)
        {
            if (x > MaxX)
                return false; // exit asap, small optimization
            if (y > MaxY)
                return false;

            if (x < MinX)
                return false;
            if (y < MinY)
                return false;

            return true;
        }
    }
}
