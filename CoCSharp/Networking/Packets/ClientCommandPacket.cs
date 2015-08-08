using System;

namespace CoCSharp.Networking.Packets
{
    public class ClientCommandPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public int Subtick;
        public int Checksum;


        public void ReadPacket(PacketReader reader)
        {
            throw new NotImplementedException();
        }

        public void WritePacket(PacketWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
