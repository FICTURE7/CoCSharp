using CoCSharp.Common;
using CoCSharp.Data;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Trap : VillageObject
    {
        public Trap(int id, int level)
        {
            this.ID = id;
            this.Level = level;
        }

        [JsonProperty("need_repair")]
        public bool Broken { get; set; }
        [JsonProperty("lvl")]
        public int Level { get; set; }
  
        public int TownHallLevel { get; set; }
        public TimeSpan BuildTime { get; set; }
        public int BuildCost { get; set; }
        public Resource BuildResource { get; set; }
        public int RearmCost { get; set; }
    }
}
