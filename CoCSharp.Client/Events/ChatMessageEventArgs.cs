using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Client.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public ChatMessageEventArgs(string message, ChatMessageServerPacket packet)
        {
            Message = message;
            Packet = packet;
        }

        public ChatMessageServerPacket Packet { get; set; }
        public string Message { get; set; }
    }
}
