using CoCSharp.Data;
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

            client.UserID = lsPacket.UserID;
            client.UserToken = lsPacket.UserToken;
            client.LoggedIn = true;
        }

        public static void HandleUpdateKeyPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var ukPacket = packet as UpdateKeyPacket;

            client.ClientNetworkManager.UpdateCiphers((ulong)client.Seed, ukPacket.Key);
            client.ClientNetworkManager.UpdateCiphers((ulong)client.Seed, ukPacket.Key);
        }

        public static void HandleLoginRequestPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var lrPacket = packet as LoginRequestPacket;

            client.Seed = lrPacket.Seed;
            client.UserID = lrPacket.UserID;
            client.UserToken = lrPacket.UserToken;
            client.FingerprintHash = lrPacket.FingerprintHash;
        }

        public static void HandleOwnHomeDataPacket(CoCProxy proxyServer, CoCProxyClient client, IPacket packet)
        {
            var ohPacket = packet as OwnHomeDataPacket;
            client.Username = ohPacket.Username;
            client.UserID = ohPacket.UserID;
            client.Home = ohPacket.Home;
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
