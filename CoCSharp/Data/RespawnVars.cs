using Newtonsoft.Json;

namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class RespawnVars
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RespawnVars"/> class.
        /// </summary>
        public RespawnVars()
        {
            // Space
        }

        [JsonProperty("obstacleClearCounter")]
        public int ObstacleClearCounter { get; set; }
        [JsonProperty("respawnSeed")]
        public int RespawnSeed { get; set; }
        [JsonProperty("secondsFromLastRespawn")]
        public int SecoundsFromLastRespawn { get; set; }
        [JsonProperty("time_in_gembox_period")]
        public int TimeInGemboxPeriod { get; set; }
        [JsonProperty("time_to_gembox_drop")]
        public int TimeToGemboxDrop { get; set; }
    }
}
