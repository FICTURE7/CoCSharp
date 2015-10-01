using System;

namespace CoCSharp.Networking.Packets
{
    public class ClanDescriptionPacket : IPacket
    {
        public ushort ID { get { return 0x5F04; } }

        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public long ClanID;
        public string ClanName;
        public int ClanBadge;
        public int ClanLevel; // not sure about this
        public int ClanMemberCount; // not sure about this
        public int ClanTrophies;
        public int Unknown5; // -+
        public int Unknown6; //  |- might wars won,lost,draw
        public int Unknown7; // -+
        public long Unknown8;
        public long Unknown9; // Unknown8 = Unknown9
        public int Unknown10;
        public int Unknown11;

        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            ClanID = reader.ReadInt64();
            ClanName = reader.ReadString();
            ClanBadge = reader.ReadInt32();
            ClanMemberCount = reader.ReadInt32();
            ClanTrophies = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt64();
            Unknown9 = reader.ReadInt64();
            Unknown10 = reader.ReadInt32();
            Unknown11 = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            
        }
    }
}
