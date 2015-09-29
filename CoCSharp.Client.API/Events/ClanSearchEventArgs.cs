using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Client.API.Events
{
    public class ClanSearchEventArgs : EventArgs
    {
        public ClanSearchEventArgs(AllianceListResponsePacket packet)
        {
            Packet = packet;
        }

        public AllianceListResponsePacket Packet { get; set; }
    }
}
