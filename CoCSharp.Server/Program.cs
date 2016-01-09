using CoCSharp.Logic;
using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
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

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void m()
        {
            var manager = new DataManager();
            var building = new Building();
            building.Data = manager.FindBuilding(1, 5);
            building.BeginConstruct();
            Console.ReadLine();
        }
    }
}
