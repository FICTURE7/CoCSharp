using CoCSharp.Networking;
using CoCSharp.Networking.Messages;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class Program
    {
        public static CoCProxy Proxy { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# - Proxy";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Directory.CreateDirectory("messages");
            Directory.CreateDirectory("villages");
            Proxy = new CoCProxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void m()
        {
            var reader = new MessageReader(new MemoryStream(File.ReadAllBytes("msg")));
            var ohdMessage = new OwnHomeDataMessage();
            ohdMessage.ReadMessage(reader);
        }
    }
}
