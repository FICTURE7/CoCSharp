using CoCSharp.Common;
using CoCSharp.Data;
using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Decoration : VillageObject
    {
        public Decoration(int id) : base(id) { }

        public int RequiredLevel { get; set; }
        public int MaxCount { get; set; }
        public int BuildCost { get; set; }
        public Resource BuildResource { get; set; }
    }
}
