using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void m()
        {
            var sw = new Stopwatch();
            var manager = new AssetManager("Content");
            manager.LoadCsv<BuildingData>("buildings.csv");

            AssetManager.Default = manager;

            var villagePath = Path.Combine(DirectoryPaths.Content, "starting_village.json");
            var villageJson = File.ReadAllText(villagePath);

            sw.Restart();
            var village = Village.FromJson(villageJson);
            sw.Stop();
            Console.WriteLine("FromJson(string): {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
