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

        public static void Main(string[] args)
        {
            Console.WriteLine("Compressing 'message-dumps' directory...");
            CompressDir("message-dumps");

            Console.WriteLine("Compressing 'village-dumps' directory...");
            CompressDir("village-dumps");

            var sw = Stopwatch.StartNew();

            Proxy = new Proxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            sw.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", sw.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        private static void CompressDir(string path)
        {
            if (!Directory.Exists(path))
                return;

            if (!Directory.Exists("compressed-dumps"))
                Directory.CreateDirectory("compressed-dumps");

            var now = DateTime.Now;
            var fileName = Path.Combine("compressed-dumps", $"{path}_{now.ToString("yy_MM_dd_hh_mm_ss")}.zip");
            using (var zip = new ZipFile(fileName))
            {
                zip.AddDirectory(path);
                zip.Save();
            }

            Directory.Delete(path, true);
        }
    }
}
