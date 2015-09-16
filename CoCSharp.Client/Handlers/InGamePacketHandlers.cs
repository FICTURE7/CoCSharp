using CoCSharp.Client.Events;
using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Client.Handlers
{
    public static class InGamePacketHandlers
    {
        public static void HandleChatMessageServerPacket(CoCClient client, IPacket packet)
        {
            var cmsPacket = packet as ChatMessageServerPacket;
            client.OnChatMessage(new ChatMessageEventArgs(cmsPacket));
        }

        public static void HandleOwnHomeDataPacket(CoCClient client, IPacket packet)
        {
            var ohdPacket = packet as OwnHomeDataPacket;
            client.Home = ohdPacket.Home;
            client.Avatar.Username = ohdPacket.Avatar.Username;
            
            if (ohdPacket.Avatar.Clan != null)
                Console.Title = string.Format("[{0}] - {1}", ohdPacket.Avatar.Clan.Name, ohdPacket.Avatar.Username);
            else
                Console.Title = string.Format("{0}", ohdPacket.Avatar.Username);
                
            Console.WriteLine("Village Info: ");
            Console.WriteLine("\tBuildings count: {0}", ohdPacket.Home.Buildings.Count);
            Console.WriteLine("\tDecorations count: {0}", ohdPacket.Home.Decorations.Count);
            Console.WriteLine("\tObstacles count: {0}", ohdPacket.Home.Obstacles.Count);
            Console.WriteLine("\tTraps count: {0}", ohdPacket.Home.Traps.Count);
            Console.WriteLine();
        }

        public static void HandleServerErrorPacket(CoCClient client, IPacket packet)
        {
            var errPacket = packet as ServerErrorPacket;
            Console.WriteLine("Server Error: {0}", errPacket.ErrorMessage);
        }

        public static void RegisterInGamePacketHandler(CoCClient client)
        {
            client.RegisterPacketHandler(new ChatMessageServerPacket(), HandleChatMessageServerPacket);
            client.RegisterPacketHandler(new OwnHomeDataPacket(), HandleOwnHomeDataPacket);
            client.RegisterPacketHandler(new ServerErrorPacket(), HandleServerErrorPacket);
        }
    }
}
