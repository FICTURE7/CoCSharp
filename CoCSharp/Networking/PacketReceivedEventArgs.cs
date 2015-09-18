using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Networking
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public PacketReceivedEventArgs(IPacket packet, Exception ex)
        {
            Packet = packet;
            Exception = ex;
        }

        public IPacket Packet { get; set; }
        public Exception Exception { get; set; }
    }
}
