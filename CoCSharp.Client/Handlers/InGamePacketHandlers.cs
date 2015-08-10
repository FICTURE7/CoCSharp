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
            client.OnChatMessage(new ChatMessageEventArgs(cmsPacket.Message, cmsPacket));
            var message = cmsPacket.ClanName == null ? 
                          string.Format("<{0}>: {1}", cmsPacket.Username, cmsPacket.Message) : 
                          string.Format("<[{0}]{1}>: {2}", cmsPacket.ClanName, cmsPacket.Username, cmsPacket.Message);
            Console.WriteLine(message);
        }

        public static void RegisterInGamePacketHandler(CoCClient client)
        {
            client.RegisterPacketHandler(new ChatMessageServerPacket(), HandleChatMessageServer);
        }
    }
}
