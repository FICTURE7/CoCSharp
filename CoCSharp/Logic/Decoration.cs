using CoCSharp.Database;
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

        public override void FromDatabase(BaseDatabase database)
        {
            var decorationDb = (DecorationDatabase)database;
            var decoration = (Decoration)null;

            decorationDb.TryGetDecoration(ID, out decoration);

            Name = decoration.Name;
            RequiredLevel = decoration.RequiredLevel;
            MaxCount = decoration.MaxCount;
            BuildCost = decoration.BuildCost;
            BuildResource = decoration.BuildResource;
        }
    }
}
