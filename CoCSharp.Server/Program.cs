using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static Server Server { get; set; }

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

        static void k()
        {
            k();
        }

        public static void Main(string[] args)
        {
            Console.ReadLine();
            throw new ArgumentException();

            FancyConsole.Enabled = false;
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("starting server...");

            Server = new Server();
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
                    Log.Exception("flushing avatars", ex);
                }

                try
                {
                    AllianceManager.Flush();
                }
                catch (Exception ex)
                {
                    Log.Exception("flushing alliances", ex);
                }

                Thread.Sleep(100);
            }
        }
    }
}
