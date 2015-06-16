using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace CoCSharp
{
    class Program
    {
        private static void Main(string[] args)
        {
            //TODO: Implement args.
            Console.WindowWidth += 20;
            Console.Title = "CoC# Proxy";

            var endPoint = new IPEndPoint(IPAddress.Any, CoCProxy.DefaultPort);

            Console.WriteLine("Initializing new instance of proxy...");
            var proxy = new CoCProxy();
            Console.WriteLine("Starting proxy...");
            proxy.Start(endPoint);
            Console.WriteLine("Proxy listening on *:{0}", endPoint.Port);

            Thread.Sleep(-1);
        }

        //private static void Main()
        //{
        //    var packetBytes = File.ReadAllBytes("lel");
        //    var reader = new PacketReader(new MemoryStream(packetBytes));
        //    var packet = new OwnHomeDataPacket();
        //    packet.ReadPacket(reader);
        //}
    }
}
