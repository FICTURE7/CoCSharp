using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class AllianceRankingListResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5F51; } }

        public List<AllianceRankingInfo> AllianceRankingList;

        public void ReadPacket(PacketReader reader)
        {
            AllianceRankingList = new List<AllianceRankingInfo>();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var alliance = new AllianceRankingInfo();
                alliance.ClanID = reader.ReadInt64();
                alliance.ClanName = reader.ReadString();
                alliance.Rank = reader.ReadInt32();
                alliance.Trophies = reader.ReadInt32();
                alliance.LastRank = reader.ReadInt32();
                alliance.Unknown1 = reader.ReadInt32();
                alliance.Members = reader.ReadInt32();
                alliance.BadgeID = reader.ReadInt32();
                alliance.ClanLevel = reader.ReadInt32();
                AllianceRankingList.Add(alliance);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            var count = AllianceRankingList.Count;
            for (int i = 0; i < count; i++)
            {
                var alliance = AllianceRankingList[i];
                writer.WriteInt64(alliance.ClanID);
                writer.WriteString(alliance.ClanName);
                writer.WriteInt32(alliance.Rank);
                writer.WriteInt32(alliance.Trophies);
                writer.WriteInt32(alliance.LastRank);
                writer.WriteInt32(alliance.Unknown1);
                writer.WriteInt32(alliance.Members);
                writer.WriteInt32(alliance.BadgeID);
                writer.WriteInt32(alliance.ClanLevel);
            }
        }
    }


    public class AllianceRankingInfo
    {
        public long ClanID { get; set; }
        public string ClanName { get; set; }
        public int Rank { get; set; }
        public int Trophies { get; set; }
        public int LastRank { get; set; }
        public int Unknown1 { get; set; }
        public int Members { get; set; }
        public int BadgeID { get; set; }
        public int ClanLevel { get; set; }
    }
}
