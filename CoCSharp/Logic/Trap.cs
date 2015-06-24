using CoCSharp.Databases;
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

        public override void FromDatabase(BaseDatabase database)
        {
            var trapDb = (TrapDatabase)database;
            var trapOut = (Trap)null;

            if (!trapDb.TryGetTrap(ID, Level, out trapOut))
                return;

            Name = trapOut.Name;
            BuildTime = trapOut.BuildTime;
            BuildCost = trapOut.BuildCost;
            BuildResource = trapOut.BuildResource;
            RearmCost = trapOut.RearmCost;
            TownHallLevel = trapOut.TownHallLevel;
        }
    }
}
