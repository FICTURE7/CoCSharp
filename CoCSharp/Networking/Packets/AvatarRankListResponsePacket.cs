using CoCSharp.Logic;
using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class AvatarRankListResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5F53; } }

        public List<MemberInfo> RankList;

        public void ReadPacket(PacketReader reader)
        {
            RankList = new List<MemberInfo>();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var memberInfo = new MemberInfo();
                memberInfo.Unknown1 = reader.ReadInt64();
                memberInfo.Name = reader.ReadString();
                memberInfo.Rank = reader.ReadInt32();
                memberInfo.Trophies = reader.ReadInt32();
                memberInfo.Unknown2 = reader.ReadInt32();
                memberInfo.Level = reader.ReadInt32();
                memberInfo.AttacksWon = reader.ReadInt32();
                memberInfo.AttacksLost = reader.ReadInt32();
                memberInfo.DefensesWon = reader.ReadInt32();
                memberInfo.DefensesLost = reader.ReadInt32();
                memberInfo.Unknown3 = reader.ReadInt32();
                memberInfo.CountryCode = reader.ReadString();
                memberInfo.Unknown4 = reader.ReadInt64();
                memberInfo.Unknown5 = reader.ReadInt64();
                if (reader.ReadBoolean())
                {
                    memberInfo.Clan = new Clan();
                    memberInfo.Clan.ID = reader.ReadInt64();
                    memberInfo.Clan.Name = reader.ReadString();
                    memberInfo.Clan.Badge = reader.ReadInt32();
                }
                RankList.Add(memberInfo);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(RankList.Count);
            for (int i = 0; i < RankList.Count; i++)
            {
                var memberInfo = RankList[i];
                writer.WriteInt64(memberInfo.Unknown1);
                writer.WriteString(memberInfo.Name);
                writer.WriteInt32(memberInfo.Rank);
                writer.WriteInt32(memberInfo.Trophies);
                writer.WriteInt32(memberInfo.Unknown2);
                writer.WriteInt32(memberInfo.Level);
                writer.WriteInt32(memberInfo.AttacksWon);
                writer.WriteInt32(memberInfo.AttacksLost);
                writer.WriteInt32(memberInfo.DefensesWon);
                writer.WriteInt32(memberInfo.DefensesLost);
                writer.WriteInt32(memberInfo.Unknown3);
                writer.WriteString(memberInfo.CountryCode);
                writer.WriteInt64(memberInfo.Unknown4);
                writer.WriteInt64(memberInfo.Unknown5);
                if (memberInfo.Clan != null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteInt64(memberInfo.Clan.ID);
                    writer.WriteString(memberInfo.Clan.Name);
                    writer.WriteInt32(memberInfo.Clan.Badge);
                }
            }
        }

        public class MemberInfo
        {
            public long Unknown1; // userId?
            public string Name;
            public int Rank;
            public int Trophies;
            public int Unknown2;
            public int Level;
            public int AttacksWon;
            public int AttacksLost;
            public int DefensesWon;
            public int DefensesLost;
            public int Unknown3;
            public string CountryCode;
            public long Unknown4; // userId?
            public long Unknown5;
            public Clan Clan;
        }
    }
}
