using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    public class Obstacle : VillageObject
    {
        [JsonProperty("loot_multiply_ver")]
        public int LootMultiplier { get; set; }
    }
}
