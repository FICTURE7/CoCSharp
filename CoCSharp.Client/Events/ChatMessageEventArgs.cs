using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Client.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public ChatMessageEventArgs(ChatMessageServerPacket packet)
        {
            Packet = packet;
            Message = packet.Message;
            Username = packet.Username;
            ClanName = packet.ClanName;
        }

        public ChatMessageServerPacket Packet { get; private set; }
        public string Message { get; private set; }
        public string Username { get; private set; }
        public string ClanName { get; private set; }
    }
}
