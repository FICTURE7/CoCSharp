using CoCSharp.Data;
using CoCSharp.Data.Csv;
using CoCSharp.Logic;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void m(string[] args)
        {
            Console.Title = "CoC# Server";

            Server = new CoCServer();
            Server.Start(new IPEndPoint(IPAddress.Any, 9339));

            Thread.Sleep(-1);
        }

        public static void Main()
        {
            var assetManager = new AssetManager(new Fingerprint(File.ReadAllText("fingerprint.json")));
            assetManager.DownloadAssets("http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/");
        }
    }
}
