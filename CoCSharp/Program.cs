using CoCSharp.Database;
using CoCSharp.Logic;
using System;
using System.Net;
using System.Threading;

namespace CoCSharp
{
    class Program
    {
        private static void Main(string[] args)
        {
            //TODO: Implement args.
            Console.WindowWidth += 20;
            Console.Title = "CoC# Proxy";

            var endPoint = new IPEndPoint(IPAddress.Any, CoCProxy.DefaultPort);

            Console.WriteLine("Initializing new instance of proxy...");
            var proxy = new CoCProxy();
            Console.WriteLine("Starting proxy...");
            proxy.Start(endPoint);
            Console.WriteLine("Proxy listening on *:{0}", endPoint.Port);

            Thread.Sleep(-1);
        }

        private static void TestObstacleMain(string[] args)
        {
            // TestObstacle
            var obstacle = (Obstacle)null;
            var obstacleDb = new ObstacleDatabase(@"database\obstacles.csv");

            obstacleDb.LoadDatabase();

            LogVillageObject(obstacle);
            Console.ReadLine();
        }

        private static void TestDecorationMain(string[] args)
        {
            var decoration = (Decoration)null;
            var decorationDb = new DecorationDatabase(@"database\decos.csv");

            decorationDb.LoadDatabase();
            decorationDb.TryGetDecoration(18000007, out decoration);

            LogVillageObject(decoration);
            Console.ReadLine();
        }

        private static void TestBuildingMain(string[] args)
        {
            var building = (Building)null;
            var buildingDb = new BuildingDatabase(@"database\buildings.csv");

            buildingDb.LoadDatabase();
            buildingDb.TryGetBuilding(1000000, 1, out building);

            LogVillageObject(building);
            Console.ReadLine();
        }

        private static void TestTrapMain(string[] args)
        {
            var trap = (Trap)null;
            var trapDb = new TrapDatabase(@"database\traps.csv");

            trapDb.LoadDatabase();
            trapDb.TryGetTrap(12000000, 1, out trap);

            LogVillageObject(trap);
            Console.ReadLine();
        }

        private static void LogVillageObject(VillageObject villageObj)
        {
            if (villageObj == null) return;
            var type = villageObj.GetType();
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                var name = info.Name;
                var value = info.GetMethod.Invoke(villageObj, null);
                Console.WriteLine("{0}: {1}", name, value);
            }
        }
    }
}
