using Newtonsoft.Json;
using System;

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
        private int m_SecoundsFromLastRespawn = 0;
        public TimeSpan SecoundsFromLastRespawn { get { return TimeSpan.FromSeconds(m_SecoundsFromLastRespawn); } }

        [JsonProperty("time_in_gembox_period")]
        private int m_TimeInGemboxPeriod = 0;
        public TimeSpan TimeInGemboxPeriod { get { return TimeSpan.FromSeconds(m_TimeInGemboxPeriod); } }

        [JsonProperty("time_to_gembox_drop")]
        private int m_TimeToGemboxDrop = 0;
        public TimeSpan TimeToGemboxDrop { get { return TimeSpan.FromSeconds(m_TimeToGemboxDrop); } }
    }
}
