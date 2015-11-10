using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class AllianceRankingListLocalResponsePacket: AllianceRankingListResponsePacket
    {

        public new ushort ID { get { return 0x5F52; } }

        public new void ReadPacket(PacketReader reader)
        {
            base.ReadPacket(reader);
        }
        public new void WritePacket(PacketWriter writer)
        {
            base.WritePacket(writer);
        }
    }
}
