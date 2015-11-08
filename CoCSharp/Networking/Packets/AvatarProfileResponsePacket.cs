using CoCSharp.Logic;
using System.IO;

namespace CoCSharp.Networking.Packets
{
    public class AvatarProfileResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5F0E; } }

        public int Unknown1;
        public long UserID1;
        public long UserID2;
        public Clan Clan;
        public bool Unknown2;
        public long Unknown3;
        public int Unknown4;
        public int AllianceCastleLevel;
        public int AllianceCastleUnitCapacity;
        public int AllianceCastleUnitCount;
        public int TownHallLevel;
        public string Username;
        public string FacebookID;
        private int Level;
        private int Experience;
        private int Gems;
        private int Gems1;
        private int Unknown6;
        private int Unknown5;
        private int DefenseLost;
        private int DefenseWon;
        private int AttackLost;
        private int AttackWon;
        private int Trophies;

        public void ReadPacket(PacketReader reader)
        {
            //TODO: Make
            var offset = 0x2A;
            File.WriteAllBytes("AvatarProfileResponsePacket dump", ((MemoryStream)reader.BaseStream).ToArray());
            Unknown1 = reader.ReadInt32();
            UserID1 = reader.ReadInt64();
            UserID2 = reader.ReadInt64();
            if (reader.ReadBoolean())
            {
                Clan = new Clan();
                Clan.ID = reader.ReadInt64();
                Clan.Name = reader.ReadString();
                Clan.Badge = reader.ReadInt32();
                reader.ReadInt32();
                Clan.Level = reader.ReadInt32();
                offset += 1;
            }
            if (Unknown2 = reader.ReadBoolean())
            {
                Unknown3 = reader.ReadInt64();
                offset += 1;
            }

            reader.Seek(offset, SeekOrigin.Current);
            Unknown4 = reader.ReadInt32();
            AllianceCastleLevel = reader.ReadInt32(); // -1 if not constructed
            AllianceCastleUnitCapacity = reader.ReadInt32();
            AllianceCastleUnitCount = reader.ReadInt32();
            TownHallLevel = reader.ReadInt32();
            Username = reader.ReadString();
            FacebookID = reader.ReadString();
            Level = reader.ReadInt32();
            Experience = reader.ReadInt32();
            Gems = reader.ReadInt32(); // they seemed randomized or scrambled for non local player
            Gems1 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Trophies = reader.ReadInt32();
            AttackWon = reader.ReadInt32();
            AttackLost = reader.ReadInt32(); // randomized
            DefenseWon = reader.ReadInt32();
            DefenseLost = reader.ReadInt32(); // randomized
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
