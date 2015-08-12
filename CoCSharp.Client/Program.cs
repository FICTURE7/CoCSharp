using CoCSharp.Networking;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace CoCSharp.Client
{
    public class Program
    {
        public static CoCClient Client { get; set; }

        public static void Main(string[] args)
        {
#if DEBUG
            args = new string[]
            {
                "gamea.clashofclans.com",
                "9339"
            };
#endif
            Client = new CoCClient();
            var ipAddresses = Dns.GetHostAddresses(args[0]);
            var port = int.Parse(args[1]);
            var endPoint = new IPEndPoint(ipAddresses[0], port);
            Console.WriteLine("Main thread: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Connecting to {0}...", endPoint.Address);
            Client.Connect(endPoint);

            while (true)
            {
                var message = Console.ReadLine();
                Client.SendChatMessage(message);
            }
        }
    }
}
