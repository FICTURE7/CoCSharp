using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;
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

            //TODO: Don't waste this thread making it sleep forever, instead use it for a queue save system.
            Thread.Sleep(Timeout.Infinite);
        }

        public static void m()
        {
            AssetManager.DefaultInstance = new AssetManager("Content");
            AssetManager.DefaultInstance.LoadCsv<BuildingData>("buildings.csv");
            AssetManager.DefaultInstance.LoadCsv<ObstacleData>("obstacles.csv");
            AssetManager.DefaultInstance.LoadCsv<TrapData>("traps.csv");
            AssetManager.DefaultInstance.LoadCsv<DecorationData>("decos.csv");

            var manager = new AvatarManager();
            var ava = manager.CreateNewAvatar();
            ava.Units.Add(new Data.Slots.UnitSlot(100, 100));

            manager.SaveAvatar(ava);
            var kek = manager.LoadAvatar(ava.Token);
        }
    }
}
