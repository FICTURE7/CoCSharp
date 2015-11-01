using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class LeagueMemberListResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5FB7; } }

        public int SeasonEndsSeconds;
        public List<LeagueMember> LeagueMemberList;


        public void ReadPacket(PacketReader reader)
        {
            LeagueMemberList = new List<LeagueMember>();
            SeasonEndsSeconds = reader.ReadInt32();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                LeagueMember leagueMember = new LeagueMember();
                leagueMember.UserID = reader.ReadInt64();
                leagueMember.Username = reader.ReadString();
                leagueMember.Rank = reader.ReadInt32();
                leagueMember.Trophies = reader.ReadInt32();
                leagueMember.LastRank = reader.ReadInt32();
                leagueMember.Unknown3 = reader.ReadInt32();
                leagueMember.AttacksWon = reader.ReadInt32();
                leagueMember.Unknown5 = reader.ReadInt32();
                leagueMember.DefensesWon = reader.ReadInt32();
                leagueMember.Unknown7 = reader.ReadInt32();
                leagueMember.Unknown8 = reader.ReadInt32();
                leagueMember.Unknown9 = reader.ReadInt32();
                leagueMember.Unknown10 = reader.ReadInt32();
                leagueMember.Unknown11 = reader.ReadInt32();
                leagueMember.HasClan = reader.ReadBoolean();
                if (leagueMember.HasClan)
                {
                    leagueMember.ClanID = reader.ReadInt64();
                    leagueMember.ClanName = reader.ReadString();
                    leagueMember.Unknown12 = reader.ReadInt32();
                }
                leagueMember.Unknown13 = reader.ReadInt32();
                leagueMember.Unknown14 = reader.ReadInt32();
                LeagueMemberList.Add(leagueMember);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(SeasonEndsSeconds);
            var count = LeagueMemberList.Count;
            for (int i = 0; i < count; i++)
            {
                var leagueMember = LeagueMemberList[i];
                writer.WriteInt64(leagueMember.UserID);
                writer.WriteString(leagueMember.Username);
                writer.WriteInt32(leagueMember.Rank);
                writer.WriteInt32(leagueMember.Trophies);
                writer.WriteInt32(leagueMember.LastRank);
                writer.WriteInt32(leagueMember.Unknown3);
                writer.WriteInt32(leagueMember.AttacksWon);
                writer.WriteInt32(leagueMember.Unknown5);
                writer.WriteInt32(leagueMember.DefensesWon);
                writer.WriteInt32(leagueMember.Unknown7);
                writer.WriteInt32(leagueMember.Unknown8);
                writer.WriteInt32(leagueMember.Unknown9);
                writer.WriteInt32(leagueMember.Unknown10);
                writer.WriteInt32(leagueMember.Unknown11);
                writer.WriteBoolean(leagueMember.HasClan);
                if (leagueMember.HasClan)
                {
                    writer.WriteInt64(leagueMember.ClanID);
                    writer.WriteString(leagueMember.ClanName);
                    writer.WriteInt32(leagueMember.Unknown12);
                }
                writer.WriteInt32(leagueMember.Unknown13);
                writer.WriteInt32(leagueMember.Unknown14);
            }
        }
    }

    public class LeagueMember
    {
        public long UserID { get; set; }
        public string Username { get; set; }
        public int Rank { get; set; }
        public int Trophies { get; set; }
        public int LastRank { get; set; }
        public int Unknown3 { get; set; }
        public int AttacksWon { get; set; }
        public int Unknown5 { get; set; } //maybe attacks lost
        public int DefensesWon { get; set; }
        public int Unknown7 { get; set; } //maybe defense lost
        public int Unknown8 { get; set; }
        public int Unknown9 { get; set; }
        public int Unknown10 { get; set; }
        public int Unknown11 { get; set; }
        public int Unknown12 { get; set; }
        public bool HasClan { get; set; }
        public long ClanID { get; set; }
        public string ClanName { get; set; }
        public int Unknown13 { get; set; }
        public int Unknown14 { get; set; }
    }

}
