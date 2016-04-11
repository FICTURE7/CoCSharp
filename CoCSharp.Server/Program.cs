using CoCSharp.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void m(string[] args)
        {
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void Main()
        {
            var downloader = new AssetDownloader("9d075b9546211da641d06f4c576aa9b9c62212fb");
            downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
            downloader.DownloadCompleted += Downloader_DownloadCompleted; ;
            downloader.DownloadAssets(string.Empty, true);
            Console.ReadLine();
        }

        private static void Downloader_DownloadCompleted(object sender, AssetDownloadCompletedEventArgs e)
        {
            Console.WriteLine("Done!");
        }

        private static void Downloader_DownloadProgressChanged(object sender, AssetDownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("Downloaded: {0}% -> {1}", Math.Round(e.ProgressPercentage, 1), e.FileDownloaded.Path);
        }
    }
}
