namespace CoCSharp.Networking.Packets
{
    public class AllianceDescriptionPacket : IPacket
    {
        public ushort ID { get { return 0x5F04; } }

        public string ClanDescription;
        public bool Unknown1;
        public int Unknown2;
        public int Unknown3;
        public long Unknown4;
        public long Unknown5;
        public string ClanName;
        public int ClanBadge;
        public int ClanJoinType; // not sure about this
        public int ClanMemberCount; // not sure about this
        public int ClanTrophies;
        public int ClanMinTrophies; 
        public int ClanWarsWon; 
        public int ClanWarsLost; 
        public int ClanWarsDraw;
        public int Unknown6; // Unknown8 = Unknown9
        public int Unknown7;
        public int Unknown8;
        public int ClanEP;
        public int ClanLevel;

        public void ReadPacket(PacketReader reader)
        {
            ClanDescription = reader.ReadString();
            Unknown1 = reader.ReadBoolean(); 
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt64();
            Unknown5 = reader.ReadInt64();
            ClanName = reader.ReadString();
            ClanBadge = reader.ReadInt32();
            ClanJoinType = reader.ReadInt32();
            ClanMemberCount = reader.ReadInt32();
            ClanTrophies = reader.ReadInt32();
            ClanMinTrophies = reader.ReadInt32();
            ClanWarsWon = reader.ReadInt32();
            ClanWarsLost = reader.ReadInt32();
            ClanWarsDraw = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            ClanEP = reader.ReadInt32();
            ClanLevel = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            
        }
    }
}
