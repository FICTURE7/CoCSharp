using CoCSharp.Server.Api;
using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static IServer Server { get; set; }

        public static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((a) => Console.WriteLine("k"));

            Console.CancelKeyPress += (sender, e) =>
            {
                if (!e.Cancel)
                {
                    if (Server != null)
                    {
                        Server.Logs.Info("Closing server...");
                        Server.Close();
                    }
                }
            };

            var sw = Stopwatch.StartNew();

            Server = new Server();
            Server.Logs.Info("Starting server...");
            Server.ClientConnected += (sender, e) =>
            {
                e.Server.Logs.Info($"New connection at {e.Client.RemoteEndPoint}");
            };
            Server.Start();

            Server.Logs.Info($"Done in {sw.Elapsed.TotalMilliseconds}ms...");

            // Wasting the main thread, because we can.
            Thread.Sleep(Timeout.Infinite);
        }       
    }
}
