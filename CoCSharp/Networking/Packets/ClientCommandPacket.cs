using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class ClientCommandPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public int Subtick;
        public int Checksum;


        public void ReadPacket(PacketReader reader)
        {

        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
