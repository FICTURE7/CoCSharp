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
            Proxy = new CoCProxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
