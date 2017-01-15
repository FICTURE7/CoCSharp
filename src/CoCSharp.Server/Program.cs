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

            Server.Logs.Info("Initialized Fluxcapacitor v6.9");
            Server.Logs.Info("Loading hastables to run Fluxcapacitor v6.9 to connect to the International Space Station.");
            Server.Logs.Info("Connecting to the custom MongoDB v4.2.0 server powered by nuclear energy (Uranium).");
            Server.Logs.Info("Hacking into the NSA to get the CSV tables.");
            Server.Logs.Info("Hacking into Supercell's Amazon Web Service to extract the latest keys.");
            Server.Logs.Info("Server is ready to start sending nuclear warheads into space and handle Clash of Clans connections.");
            Server.Logs.Info($"Done in {sw.Elapsed.TotalMilliseconds}ms...");

            int nsa = 69696969;
            hack(nsa);

            // Wasting the main thread, because we can.
            Thread.Sleep(Timeout.Infinite);
        }

        static void hack(int k)
        {
            // hacking...
        }
    }
}
