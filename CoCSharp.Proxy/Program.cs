using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class Program
    { 
        public static CoCProxy Proxy { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# Proxy";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Proxy = new CoCProxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            stopwatch.Stop();

            Console.WriteLine("Running({0}ms) on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(-1);
        }
    }
}
