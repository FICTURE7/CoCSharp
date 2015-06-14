using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    public class Building : VillageObject
    {
        [JsonProperty("hp")]
        public int Health { get; set; }
    }
}
