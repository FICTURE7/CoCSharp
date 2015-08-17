using CoCSharp.Client.Events;
using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Client.Handlers
{
    public static class InGamePacketHandlers
    {
        public static void HandleChatMessageServer(CoCClient client, IPacket packet)
        {
            var cmsPacket = packet as ChatMessageServerPacket;
            client.OnChatMessage(new ChatMessageEventArgs(cmsPacket));
        }

        public static void RegisterInGamePacketHandler(CoCClient client)
        {
            client.RegisterPacketHandler(new ChatMessageServerPacket(), HandleChatMessageServer);
        }
    }
}
