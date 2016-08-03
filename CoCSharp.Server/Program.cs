using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static AvatarManager AvatarManager
        {
            get
            {
                if (Server == null)
                    return null;

                return Server.AvatarManager;
            }
        }

        public static AllianceManager AllianceManager
        {
            get
            {
                if (Server == null)
                    return null;

                return Server.AllianceManager;
            }
        }

        public static void Main(string[] args)
        {
            FancyConsole.Enabled = false;
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("done({0}ms): listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Console.WriteLine();
            while (true)
            {
                try
                {
                    AvatarManager.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXCEPTION: flushing avatars: {0}", ex.Message);
                    Console.WriteLine();
                }

                try
                {
                    AllianceManager.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXCEPTION: flushing alliances: {0}", ex.Message);
                    Console.WriteLine();
                }

                Thread.Sleep(100);
            }
        }

        public static void m()
        {
            Console.WriteLine("-> Setting up AssetManager...");
            var AssetManager = new AssetManager(DirectoryPaths.Content);

            Console.WriteLine("     > Loading buildings.csv...");
            AssetManager.LoadCsv<BuildingData>("buildings.csv");
            Console.WriteLine("     > Loading traps.csv...");
            AssetManager.LoadCsv<TrapData>("traps.csv");
            Console.WriteLine("     > Loading obstacles.csv...");
            AssetManager.LoadCsv<ObstacleData>("obstacles.csv");
            Console.WriteLine("     > Loading decos.csv...");
            AssetManager.LoadCsv<DecorationData>("decos.csv");
            Console.WriteLine("     > Loading resources.csv...");
            AssetManager.LoadCsv<ResourceData>("resources.csv");

            AssetManager.DefaultInstance = AssetManager;
        }
    }
}
