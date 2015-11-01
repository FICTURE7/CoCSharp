using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class LeagueMemberListRequestPacket : IPacket
    {
        public ushort ID { get { return 0x38A7; } }
        public bool Unknown1;
        public long Unknown2;


        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadBoolean();
            Unknown2 = reader.ReadInt64();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteBoolean(Unknown1);
            writer.WriteInt64(Unknown2);
        }
    }
}
