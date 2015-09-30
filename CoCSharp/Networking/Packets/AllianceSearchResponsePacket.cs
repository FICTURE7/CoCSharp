namespace CoCSharp.Networking.Packets
{
    public class AllianceSearchResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5EF6; } }

        public string SearchString;
        public ClanSearchInfo[] ClansSearchInfo;

        public void ReadPacket(PacketReader reader)
        {
            SearchString = reader.ReadString();
            ClansSearchInfo = new ClanSearchInfo[reader.ReadInt32()];
            for (int i = 0; i < ClansSearchInfo.Length; i++)
            {
                ClanSearchInfo info = new ClanSearchInfo();
                info.ID = reader.ReadInt64();
                info.Name = reader.ReadString();
                info.Unknown1 = reader.ReadInt32();
                info.Type = reader.ReadInt32();
                info.MemberCount = reader.ReadInt32();
                info.Trophies = reader.ReadInt32();
                info.Unknown2 = reader.ReadInt32();
                info.WarsWon = reader.ReadInt32();
                info.WarsLost = reader.ReadInt32();
                info.WarsDraw = reader.ReadInt32();
                info.Badge = reader.ReadInt32();
                info.Unknown3 = reader.ReadInt32();
                info.Unknown4 = reader.ReadInt32();
                info.EP = reader.ReadInt32();
                info.Level = reader.ReadInt32();
                ClansSearchInfo[i] = info;
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(SearchString);
            writer.WriteInt32(ClansSearchInfo.Length);
            for (int i = 0; i < ClansSearchInfo.Length; i++)
            {
                var info = ClansSearchInfo[i];
                writer.WriteInt64(info.ID);
                writer.WriteString(info.Name);
                writer.WriteInt32(info.Unknown1);
                writer.WriteInt32(info.Type);
                writer.WriteInt32(info.MemberCount);
                writer.WriteInt32(info.Trophies);
                writer.WriteInt32(info.Unknown2);
                writer.WriteInt32(info.WarsWon);
                writer.WriteInt32(info.WarsLost);
                writer.WriteInt32(info.WarsDraw);
                writer.WriteInt32(info.Badge);
                writer.WriteInt32(info.Unknown3);
                writer.WriteInt32(info.Unknown4);
                writer.WriteInt32(info.EP);
                writer.WriteInt32(info.Level);
            }
        }

        public class ClanSearchInfo
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public int Unknown1 { get; set; }
            public int Type { get; set; }
            public int MemberCount { get; set; }
            public int Trophies { get; set; }
            public int Unknown2 { get; set; }
            public int WarsWon { get; set; }
            public int WarsLost { get; set; }
            public int WarsDraw { get; set; }
            public int Badge { get; set; }
            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int EP { get; set; }
            public int Level { get; set; }
        }
    }
}
