using CoCSharp.Csv;
using CoCSharp.Data;
using System.Linq;

namespace CoCSharp.Server.Core
{
    public class DataManager
    {
        // TODO: Make this safer.

        public DataManager()
        {
            var buildingTable = new CsvTable("Content\\buildings.csv");
            BuildingsData = CsvConvert.Deserialize<BuildingData>(buildingTable);

            var trapTable = new CsvTable("Content\\traps.csv");
            TrapsData = CsvConvert.Deserialize<TrapData>(trapTable);

            var obstacleTable = new CsvTable("Content\\obstacles.csv");
            ObstaclesData = CsvConvert.Deserialize<ObstacleData>(obstacleTable);
        }

        public BuildingData[] BuildingsData { get; set; }
        public TrapData[] TrapsData { get; set; }
        public ObstacleData[] ObstaclesData { get; private set; }

        public BuildingData[] FindBuilding(int id)
        {
            return BuildingsData.Where(bd => bd.ID == id).ToArray();
        }

        public BuildingData FindBuilding(int id, int level)
        {
            return FindBuilding(id)[level];
        }

        public TrapData[] FindTrap(int id)
        {
            return TrapsData.Where(bd => bd.ID == id).ToArray();
        }

        public TrapData FindTrap(int id, int level)
        {
            return FindTrap(id)[level];
        }

        public ObstacleData FindObstacle(int id)
        {
            return ObstaclesData.Where(bd => bd.ID == id).FirstOrDefault();
        }
    }
}
