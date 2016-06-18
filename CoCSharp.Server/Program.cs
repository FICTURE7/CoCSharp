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

        public static void m(string[] args)
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

        public static void Main()
        {
            var villageJson = File.ReadAllText("village.json");

            var sw = new Stopwatch();

            sw.Start();
            var village = Village.FromJson(villageJson);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);

            sw.Start();
            var json = village.ToJson();
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
