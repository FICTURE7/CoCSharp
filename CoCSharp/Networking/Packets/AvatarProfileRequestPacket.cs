using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class AvatarProfileRequestPacket : IPacket
    {
        public ushort ID { get { return 0x37F5; } }

        public ulong UserID;
        public ulong UserID2;
        private byte Unknown1;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadULong();
            UserID2 = reader.ReadULong();
            Unknown1 = (byte)reader.ReadByte();
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
