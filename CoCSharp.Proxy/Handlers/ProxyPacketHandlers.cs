using CoCSharp.Data;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Net.Sockets;

namespace CoCSharp.Proxy.Handlers
{
    public static class ProxyPacketHandlers
    {
        public static void HandleLoginSuccessPacket(CoCProxy proxyServer, CoCProxyConnection client, IPacket packet)
        {
            var lsPacket = packet as LoginSuccessPacket;

            client.Client.UserID = lsPacket.UserID;
            client.Client.UserToken = lsPacket.UserToken;
            client.Client.LoggedIn = true;
        }

        public static void HandleUpdateKeyPacket(CoCProxy proxyServer, CoCProxyConnection client, IPacket packet)
        {
            var ukPacket = packet as UpdateKeyPacket;

            client.Client.ClientNetworkManager.UpdateChipers((ulong)client.Client.Seed, ukPacket.Key);
            client.Server.ClientNetworkManager.UpdateChipers((ulong)client.Client.Seed, ukPacket.Key);
        }

        public static void HandleLoginRequestPacket(CoCProxy proxyServer, CoCProxyConnection client, IPacket packet)
        {
            var lrPacket = packet as LoginRequestPacket;

            client.Start((ICoCServer)proxyServer, new TcpClient(proxyServer.ServerAddress, proxyServer.ServerPort).Client);
            client.Client.Seed = lrPacket.Seed;
            client.Client.UserID = lrPacket.UserID;
            client.Client.UserToken = lrPacket.UserToken;
            client.Client.FingerprintHash = lrPacket.FingerprintHash;

            if (!proxyServer.DatabaseManagers.ContainsKey(lrPacket.FingerprintHash))
            {
                var dbManager = new DatabaseManager(lrPacket.FingerprintHash);
                proxyServer.RegisterDatabaseManager(dbManager, lrPacket.FingerprintHash);
            }
        }

        public static void HandleOwnHomeDataPacket(CoCProxy proxyServer, CoCProxyConnection client, IPacket packet)
        {
            var ohPacket = packet as OwnHomeDataPacket;
            client.Client.Username = ohPacket.Username;
            client.Client.UserID = ohPacket.UserID;
            client.Client.Home = ohPacket.Home;

            // load buildings, traps, decorations, obstacles from db
            var dbManager = (DatabaseManager)null;
            if (proxyServer.DatabaseManagers.TryGetValue(client.Client.FingerprintHash, out dbManager))
            {
                if (dbManager.IsDownloading) return;

                client.Client.Home.FromDatabase(dbManager);
            }
        }

        public static void RegisterHanlders(CoCProxy proxyServer)
        {
            proxyServer.RegisterPacketHandler(new UpdateKeyPacket(), HandleUpdateKeyPacket);
            proxyServer.RegisterPacketHandler(new LoginRequestPacket(), HandleLoginRequestPacket);
            proxyServer.RegisterPacketHandler(new LoginSuccessPacket(), HandleLoginSuccessPacket);
            proxyServer.RegisterPacketHandler(new OwnHomeDataPacket(), HandleOwnHomeDataPacket);
        }
    }
}
