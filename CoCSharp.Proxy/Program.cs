using Ionic.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCProxy Proxy { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# - Proxy";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!Directory.Exists("messages"))
                Directory.CreateDirectory("messages");
            else
            {
                Console.Write("Compressing old message dumps...");
                var files = Directory.GetFiles("messages");
                using (var messageZip = new ZipFile("messages-" + DateTime.Now.ToString("hh-mm-ss.fff") + ".zip"))
                {
                    messageZip.AddFiles(files);
                    messageZip.Save();
                }
                Directory.Delete("messages", true);
                Console.WriteLine("Done!");
                Directory.CreateDirectory("messages");
            }

            if (!Directory.Exists("villages"))
                Directory.CreateDirectory("villages");

            Proxy = new CoCProxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
