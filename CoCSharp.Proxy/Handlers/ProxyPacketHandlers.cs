using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Net.Sockets;

namespace CoCSharp.Proxy.Handlers
{
    public static class ProxyPacketHandlers
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

            client.Client.NetworkManager.UpdateCiphers((ulong)client.Client.Seed, ukPacket.Key);
            client.Server.NetworkManager.UpdateCiphers((ulong)client.Client.Seed, ukPacket.Key);
        }

        public static void HandleLoginRequestPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var lrPacket = packet as LoginRequestPacket;

            client.Start(new TcpClient(proxyServer.ServerAddress, proxyServer.ServerPort).Client);
            client.Server.NetworkManager.ExceptionLog = proxyServer.ExceptionLog;
            client.Client.Seed = lrPacket.Seed;
            client.Client.UserID = lrPacket.UserID;
            client.Client.UserToken = lrPacket.UserToken;
            client.Client.FingerprintHash = lrPacket.FingerprintHash;
        }

        public static void HandleOwnHomeDataPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var ohPacket = packet as OwnHomeDataPacket;
            client.Client.Username = ohPacket.Avatar.Username;
            client.Client.UserID = ohPacket.UserID;
            client.Client.Home = ohPacket.Home;
        }

        public static void RegisterHandlers(CoCProxy proxyServer)
        {
            proxyServer.RegisterPacketHandler(new UpdateKeyPacket(), HandleUpdateKeyPacket);
            proxyServer.RegisterPacketHandler(new LoginRequestPacket(), HandleLoginRequestPacket);
            proxyServer.RegisterPacketHandler(new LoginSuccessPacket(), HandleLoginSuccessPacket);
            proxyServer.RegisterPacketHandler(new OwnHomeDataPacket(), HandleOwnHomeDataPacket);
        }
    }
}

