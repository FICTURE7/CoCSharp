using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Server.Handlers
{
    public static class InGamePacketHandler
    {
        public static void HandleKeepAliveRequestPacket(CoCRemoteClient client, CoCServer server, IPacket packet)
        {
            client.QueuePacket(new KeepAliveResponsePacket());
        }

        public static void RegisterInGamePacketHandlers(CoCRemoteClient client)
        {
            client.RegisterPacketHandler(new KeepAliveRequestPacket(), HandleKeepAliveRequestPacket);
        }
    }
}
