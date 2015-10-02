namespace CoCSharp.Networking.Packets
{
    public class AllianceDescriptionPacket : IPacket
    {
        public ushort ID { get { return 0x5F04; } }

        public string ClanDescription;
        public int Unknown1;
        public int Unknown2;
        public bool Unknown3;
        public long Unknown4;
        public long ClanID;
        public string ClanName;
        public int ClanBadge;
        public int ClanJoinType;
        public int ClanMemberCount;
        public int ClanTrophies;
        public int ClanMinTrophies;
        public int ClanWarsWon;
        public int ClanWarsLost;
        public int ClanWarsDraw;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int ClanEP;
        public int ClanLevel;

        public void ReadPacket(PacketReader reader)
        {
            ClanDescription = reader.ReadString();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadBoolean();
            if (Unknown3)
                Unknown4 = reader.ReadInt64();
            ClanID = reader.ReadInt64();
            ClanName = reader.ReadString();
            ClanBadge = reader.ReadInt32();
            ClanJoinType = reader.ReadInt32();
            ClanMemberCount = reader.ReadInt32();
            ClanTrophies = reader.ReadInt32();
            ClanMinTrophies = reader.ReadInt32();
            ClanWarsWon = reader.ReadInt32();
            ClanWarsLost = reader.ReadInt32();
            ClanWarsDraw = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            ClanEP = reader.ReadInt32();
            ClanLevel = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(ClanDescription);
            writer.WriteInt32(Unknown1);
            writer.WriteInt32(Unknown2);
            writer.WriteBoolean(Unknown3);
            if (Unknown3)
                writer.WriteInt64(Unknown4);
            writer.WriteInt64(ClanID);
            writer.WriteString(ClanName);
            writer.WriteInt32(ClanBadge);
            writer.WriteInt32(ClanJoinType);
            writer.WriteInt32(ClanMemberCount);
            writer.WriteInt32(ClanTrophies);
            writer.WriteInt32(ClanMinTrophies);
            writer.WriteInt32(ClanWarsWon);
            writer.WriteInt32(ClanWarsLost);
            writer.WriteInt32(ClanWarsDraw);
            writer.WriteInt32(Unknown5);
            writer.WriteInt32(Unknown6);
            writer.WriteInt32(Unknown7);
            writer.WriteInt32(ClanEP);
            writer.WriteInt32(ClanLevel);
        }
    }
}
