using System;

namespace CoCSharp.Networking.Packets
{
    public class AllianceSearchRequestPacket : IPacket
    {
        public ushort ID { get { return 0x37F4; } }

        public string SearchString;

        public void ReadPacket(PacketReader reader)
        {
            SearchString = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(SearchString);
        }
    }
}
