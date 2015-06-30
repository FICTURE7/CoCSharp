using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Net.Sockets;

namespace CoCSharp.Proxy.Handlers
{
    public static class BasicPacketHandlers
    {
        public static void HandleLoginSuccessPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var lsPacket = packet as LoginSuccessPacket;

            client.Client.UserID = lsPacket.UserID;
            client.Client.UserToken = lsPacket.UserToken;
            client.Client.LoggedIn = true;
        }

        public static void HandleUpdateKeyPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var ukPacket = packet as UpdateKeyPacket;

            client.Client.NetworkManager.UpdateChipers((ulong)client.Client.Seed, ukPacket.Key);
            client.Server.NetworkManager.UpdateChipers((ulong)client.Client.Seed, ukPacket.Key);
        }

        public static void HandleLoginRequestPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var lrPacket = packet as LoginRequestPacket;

            client.Start(new TcpClient(proxyServer.ServerAddress, proxyServer.ServerPort).Client);
            client.Client.Seed = lrPacket.Seed;
            client.Client.UserID = lrPacket.UserID;
            client.Client.UserToken = lrPacket.UserToken;
        }

        public static void HandleOwnHomeDataPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var ohPacket = packet as OwnHomeDataPacket;
            client.Client.Username = ohPacket.Username;
            client.Client.UserID = ohPacket.UserID;
            client.Client.Home = ohPacket.Home;

            // load buildings, traps, decorations, obstacles from db
            for (int x = 0; x < client.Client.Home.Buildings.Count; x++)
                client.Client.Home.Buildings[x].FromDatabase(proxyServer.BuildingDatabase);

            for (int x = 0; x < client.Client.Home.Traps.Count; x++)
                client.Client.Home.Traps[x].FromDatabase(proxyServer.TrapDatabase);

            for (int x = 0; x < client.Client.Home.Decorations.Count; x++)
                client.Client.Home.Decorations[x].FromDatabase(proxyServer.DecorationDatabase);

            for (int x = 0; x < client.Client.Home.Obstacles.Count; x++)
                client.Client.Home.Obstacles[x].FromDatabase(proxyServer.ObstacleDatabase);
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
