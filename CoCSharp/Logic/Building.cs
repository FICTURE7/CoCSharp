using CoCSharp.Databases;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Building : VillageObject
    {
        public Building(int id, int level)
        {
            this.ID = id;
            this.Level = level;
        }

        [JsonProperty("hp")] // not sure about this
        public int Hitpoints { get; set; }
        [JsonProperty("lvl")]
        public int Level { get; set; }

        public int TownHallLevel { get; set; }
        public TimeSpan BuildTime { get; set; }
        public int BuildCost { get; set; }
        public Resource BuildResource { get; set; }

        public override void FromDatabase(BaseDatabase database)
        {
            var buildingDb = (BuildingDatabase)database;
            var buildingOut = (Building)null;

            if (!buildingDb.TryGetBuilding(ID, Level, out buildingOut))
                return; // building not in database

            Name = buildingOut.Name;
            Hitpoints = buildingOut.Hitpoints;
            BuildTime = buildingOut.BuildTime;
            BuildCost = buildingOut.BuildCost;
            BuildResource = buildingOut.BuildResource;
            TownHallLevel = buildingOut.TownHallLevel;
        }
    }
}
