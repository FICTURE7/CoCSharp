using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class AllianceRankingListRequestPacket : IPacket
    {
        public ushort ID { get { return 0x3841; } }

        public bool Unknown1;
        public bool Unknown2;
        public int Unknown3;
        public int Unknown4;


        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadBoolean();
            Unknown2 = reader.ReadBoolean();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteBoolean(Unknown1);
            writer.WriteBoolean(Unknown2);
            writer.WriteInt32(Unknown3);
            writer.WriteInt32(Unknown3);
        }
    }
}
