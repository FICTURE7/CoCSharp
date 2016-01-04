using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans village.
    /// </summary>
    public class Village
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class.
        /// </summary>
        public Village()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the raw json from which the <see cref="Village"/> was
        /// deserialized.
        /// </summary>
        public string RawJson { get; set; }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        public static Village FromJson(string value)
        {
            var village = JsonConvert.DeserializeObject<Village>(value);
            village.RawJson = value;
            return village;
        }

        /// <summary>
        /// Returns a JSON formatted string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A JSON formatted string that represents the current <see cref="Village"/>.</returns>
        public string ToJson()
        {
            if (RawJson != null)
                return RawJson;
            return JsonConvert.SerializeObject(this);
        }
    }
}
