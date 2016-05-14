using CoCSharp.Csv;
using CoCSharp.Data;
using System.Linq;

namespace CoCSharp.Server.Core
{
    public class DataManager
    {
        // TODO: Make this safer.
        // TODO: Avoid using LINQ for better performance.

        public DataManager()
        {
            var buildingTable = new CsvTable("Content\\buildings.csv");
            BuildingsData = CsvConvert.Deserialize<BuildingData>(buildingTable);

            var trapTable = new CsvTable("Content\\traps.csv");
            TrapsData = CsvConvert.Deserialize<TrapData>(trapTable);

            var obstacleTable = new CsvTable("Content\\obstacles.csv");
            ObstaclesData = CsvConvert.Deserialize<ObstacleData>(obstacleTable);

            var decorationTable = new CsvTable("Content\\decos.csv");
            DecorationsData = CsvConvert.Deserialize<DecorationData>(decorationTable);

            var resourceTable = new CsvTable("Content\\resources.csv");
            ResourcesData = CsvConvert.Deserialize<ResourceData>(resourceTable);
        }

        public BuildingData[] BuildingsData { get; set; }
        public TrapData[] TrapsData { get; set; }
        public ObstacleData[] ObstaclesData { get; private set; }
        public DecorationData[] DecorationsData { get; private set; }
        public ResourceData[] ResourcesData { get; private set; }

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

        public DecorationData FindDecoration(int id)
        {
            return DecorationsData.Where(bd => bd.ID == id).FirstOrDefault();
        }

        public ResourceData FindResource(string tid)
        {
            return ResourcesData.Where(rd => rd.TID == tid).FirstOrDefault();
        }
    }
}
