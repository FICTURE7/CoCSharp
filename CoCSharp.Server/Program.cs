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
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            while (true)
            {
                AvatarManager.Flush();
                AllianceManager.Flush();
                Thread.Sleep(100);
            }
        }
    }
}
