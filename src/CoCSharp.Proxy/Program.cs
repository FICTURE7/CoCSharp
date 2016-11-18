using CoCSharp.Network;
using CoCSharp.Network.Messages;
using Ionic.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class Program
    {
        public static Proxy Proxy { get; set; }

        public static void m()
        {
            var k = File.ReadAllBytes("dump.bin");
            using (var k2 = new MessageReader(new MemoryStream(k)))
            {
                var asdf = new VisitHomeDataMessage();
                asdf.ReadMessage(k2);
            }
        }

        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (!Directory.Exists("messages"))
                Directory.CreateDirectory("messages");
            else
            {
                var files = Directory.GetFiles("messages");
                if (files.Length != 0)
                {
                    Console.Write("Compressing old message dumps...");
                    using (var messageZip = new ZipFile("messages-" + DateTime.Now.ToString("hh-mm-ss.fff") + ".zip"))
                    {
                        messageZip.AddFiles(files);
                        messageZip.Save();
                    }
                    Directory.Delete("messages", true);
                    Console.WriteLine("Done!");
                }
                Directory.CreateDirectory("messages");
            }

            if (!Directory.Exists("villages"))
                Directory.CreateDirectory("villages");
            else
            {
                var files = Directory.GetFiles("villages");
                if (files.Length != 0)
                {
                    Console.Write("Compressing old message dumps...");
                    using (var villageZip = new ZipFile("villages-" + DateTime.Now.ToString("hh-mm-ss.fff") + ".zip"))
                    {
                        villageZip.AddFiles(files);
                        villageZip.Save();
                    }
                    Directory.Delete("villages", true);
                    Console.WriteLine("Done!");
                }
                Directory.CreateDirectory("villages");
            }

            Proxy = new Proxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
