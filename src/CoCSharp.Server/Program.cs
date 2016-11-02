using CoCSharp.Server.API;
using CoCSharp.Server.API.Events.Server;
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
            Console.CancelKeyPress += CancelKeyPress;

            var sw = Stopwatch.StartNew();
            Server = new Server();
            Server.Log.Info("Starting server...");
            Server.ClientConnected += OnNewConnection;
            Server.Start();

            sw.Stop();

            Server.Log.Info($"Done in {sw.Elapsed.TotalMilliseconds}ms...");

            // Wasting the main thread, because we can.
            Thread.Sleep(Timeout.Infinite);
        }

        private static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (!e.Cancel)
            {
                Server.Log.Info("Closing server...");
                Server.Close();
            }
        }

        private static void OnNewConnection(object sender, ServerConnectionEventArgs e)
        {
            e.Server.Log.Info($"New connection {e.Client.Connection.RemoteEndPoint}");
        }
    }
}
