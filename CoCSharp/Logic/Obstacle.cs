using CoCSharp.Common;
using CoCSharp.Data;
using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Obstacle : VillageObject
    {
        public Obstacle(int id) : base(id) { }

        [JsonProperty("loot_multiply_ver")]
        public int LootMultiplier { get; set; }
        public Resource ClearResource { get; set; }
        public int ClearCost { get; set; }
        public Resource LootResource { get; set; }
        public int LootCount { get; set; }
        public int RespawnWeight { get; set; }
    }
}
