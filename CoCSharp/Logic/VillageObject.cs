using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    public abstract class VillageObject
    {
        [JsonProperty("data")]
        public int ID { get; set; }

        [JsonProperty("lvl")]
        public virtual int Level { get; set; }

        [JsonProperty("x")]
        public virtual int X { get; set; }

        [JsonProperty("y")]
        public virtual int Y { get; set; }
    }
}
