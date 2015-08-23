using System.Collections.Generic;
namespace CoCSharp.Networking.Packets
{
    public class AllianceInfoResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5EED; } }

        public long ClanID;
        public string ClanName;
        public int Unknown1;
        public int Unknown2;
        public int WarsWon;
        public int TotalPoints;
        public int RequiedTrophies;
        public int MembersCount;
        public int Level;
        public int Unknown3;
        public int Sheild;
        public int WarFrequency;
        public int Unknown4;
        public int ClanPerksPoints;
        public int Unknown5;
        public string Description;
        public int Unknown6;
        public byte Unknown7;
        public int Unknown8;
        public int Unknown9;
        public List<AllianceMemberInfo> Members;

        public void ReadPacket(PacketReader reader)
        {
            ClanID = reader.ReadInt64();
            ClanName = reader.ReadString();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            WarsWon = reader.ReadInt32();
            TotalPoints = reader.ReadInt32();
            RequiedTrophies = reader.ReadInt32();
            MembersCount = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Level = reader.ReadInt32();
            Sheild = reader.ReadInt32();
            WarFrequency = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            ClanPerksPoints = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Description = reader.ReadString();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadByte();

            var count = reader.ReadInt32();
            Members = new List<AllianceMemberInfo>();
            for (int i = 0; i < count; i++)
            {
                var allianceMember = new AllianceMemberInfo();
                allianceMember.UserID = reader.ReadInt64();
                allianceMember.Name = reader.ReadString();
                allianceMember.Role = reader.ReadInt32();
                allianceMember.Level = reader.ReadInt32();
                allianceMember.League = reader.ReadInt32();
                allianceMember.Trophies = reader.ReadInt32();
                allianceMember.TroopsDonated = reader.ReadInt32();
                allianceMember.TroopsReceived = reader.ReadInt32();
                allianceMember.Rank = reader.ReadInt32();
                allianceMember.PreviousRank = reader.ReadInt32();
                allianceMember.NewMember = reader.ReadByte();
                allianceMember.ClanWarPreference = reader.ReadInt32();
                allianceMember.ClanWarPreference1 = reader.ReadInt32();
                allianceMember.Unknown1 = reader.ReadByte();
                allianceMember.UserID1 = reader.ReadInt64();
                Members.Add(allianceMember);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(ClanID);
            writer.WriteString(ClanName);
            writer.WriteInt32(Unknown1);
            writer.WriteInt32(Unknown2);
            writer.WriteInt32(WarsWon);
            writer.WriteInt32(TotalPoints);
            writer.WriteInt32(RequiedTrophies);
            writer.WriteInt32(MembersCount);
            writer.WriteInt32(Level);
        }

        public class AllianceMemberInfo
        {
            public long UserID;
            public string Name;
            public int Role;
            public int Level;
            public int League;
            public int Trophies;
            public int TroopsDonated;
            public int TroopsReceived;
            public int Rank;
            public int PreviousRank;
            public byte NewMember;
            public int ClanWarPreference;
            public int ClanWarPreference1;
            public byte Unknown1;
            public long UserID1;
        }
    }
}
