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

        public long UserID;
        private long UserID2;
        private byte Unknown1;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadLong();
            UserID2 = reader.ReadLong();
            Unknown1 = (byte)reader.ReadByte();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteLong(UserID);
            writer.WriteLong(UserID2);
            writer.WriteByte(Unknown1);
        }
    }
}
