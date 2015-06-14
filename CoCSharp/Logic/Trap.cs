using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    public class Trap : VillageObject
    {
        [JsonProperty("need_repair")]
        public bool Broken { get; set; }
    }
}
