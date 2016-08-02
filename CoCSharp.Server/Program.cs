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

            Console.WriteLine("Starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            while (true)
            {
                try
                {
                    AvatarManager.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while flushing avatars: ", ex.Message);
                    Console.WriteLine();
                }

                try
                {
                    AllianceManager.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while flushing alliances: ", ex.Message);
                    Console.WriteLine();
                }

                Thread.Sleep(100);
            }
        }
    }
}
