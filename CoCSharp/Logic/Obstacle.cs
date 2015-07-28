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

        public override void FromDatabase(Database database)
        {
            var obstacleDb = (ObstacleDatabase)database;
            var obstacle = (Obstacle)null;

            if (!obstacleDb.TryGetObstacle(ID, out obstacle))
                return;

            Name = obstacle.Name;
            ClearResource = obstacle.ClearResource;
            ClearCost = obstacle.ClearCost;
            LootResource = obstacle.LootResource;
            LootCount = obstacle.LootCount;
            RespawnWeight = obstacle.RespawnWeight;
        }
    }
}
