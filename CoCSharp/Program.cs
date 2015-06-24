using CoCSharp.Handlers;
using CoCSharp.Logic;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace CoCSharp
{
    public class Program
    {
        public static ProxyConfiguration ProxyConfiguration { get; set; }
        public static CoCProxyServer Proxy { get; set; }

        public static void Main(string[] args)
        {
            Console.WindowWidth += 20;
            Console.Title = "CoC# Proxy";

            Console.WriteLine("Starting proxy...");

            ProxyConfiguration = ProxyConfiguration.LoadConfiguration("proxyConfig.xml");

            if (ProxyConfiguration.DeleteLogOnStartup) File.Delete("packets.log");

            Proxy = new CoCProxyServer();
            Proxy.PacketLogger.LogPrivateFields = ProxyConfiguration.LogPrivatePacketFields;
            Proxy.PacketDumper.Active = ProxyConfiguration.LogRawPacket;
            Proxy.ServerAddress = ProxyConfiguration.ServerAddress;
            Proxy.ServerPort = ProxyConfiguration.ServerPort;

            var proxyEndPoint = new IPEndPoint(IPAddress.Any, ProxyConfiguration.ProxyPort);
            Proxy.Start(proxyEndPoint);
            PacketHandlers.RegisterHanlders(Proxy);

            Console.WriteLine("CoC# is running on *:{0}", proxyEndPoint.Port);
            Thread.Sleep(-1);
        }

        private static IPEndPoint ResolveAddress(string address, int port)
        {
            var entry = Dns.GetHostEntry(address);
            return new IPEndPoint(entry.AddressList[0], port);
        }

        private static void LogVillageObject(VillageObject villageObj)
        {
            if (villageObj == null) return;
            var type = villageObj.GetType();
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                var name = info.Name;
                var value = info.GetMethod.Invoke(villageObj, null);
                Console.WriteLine("{0}: {1}", name, value);
            }
        }
    }
}
