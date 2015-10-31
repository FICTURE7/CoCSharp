using CoCSharp.Logic;
using System;
using System.IO;

namespace CoCSharp.Networking.Packets
{
    public class VisitHomeResponsePacket : IPacket
    {
        public ushort ID { get { return 0x5E31; } }

        public int Unknown1;
        public int Unknown2;
        public long UserID;
        public TimeSpan Shield;
        public int Unknown3;
        public int Unknown4;

        public bool Compressed;
        public Village Home;
        public int Unknown5;
        public long UserID1;
        public long UserID2;

        public Clan Clan;
        public int Unknown6;

        public bool Unknown7;
        public long Unknown8;

        public bool Unknown9;
        public long Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int ClanUnits;
        public int TownHall;
        public string Username;
        public string FacebookID;
        public int Stars;
        public int Points;
        public int Unknown14;
        public int Unknown15;
        public int Unknown16;
        public int Unknown17;
        public int Trophies;
        public int AttacksWon;
        public int Unknown18;
        public int DefenseWon;
        public int Unknown19;
        public int Unknown20;
        public int Unknown21;
        public int Unknown22;

        public bool Unknown23;
        public long Unknown24;
        public byte Unknown25;
        public int Unknown26;
        public int Unknown27;
        public int Unknown28;
        public int Unknown29;


        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            UserID = reader.ReadInt64();
            Shield = TimeSpan.FromSeconds(reader.ReadInt32());
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Compressed = reader.ReadBoolean();
            Home = new Village();
            Home.Read(reader);
            Unknown5 = reader.ReadInt32();

            UserID1 = reader.ReadInt64();
            UserID2 = reader.ReadInt64();

            var offset = 0x2A;
            bool isClan = reader.ReadBoolean();
            if (isClan)
            {
                Clan = new Clan();
                Clan.ID = reader.ReadInt64();
                Clan.Name = reader.ReadString();
                Clan.Badge = reader.ReadInt32();
                Unknown6 = reader.ReadInt32();
                Clan.Level = reader.ReadInt32();
                offset += 1;
            }

            bool Unknown7 = reader.ReadBoolean();
            if (Unknown7)
                Unknown8 = reader.ReadInt64();

            bool Unknown9 = reader.ReadBoolean();
            if (Unknown9)
                Unknown10 = reader.ReadInt64();

            reader.Seek(offset, SeekOrigin.Current);
            Unknown11 = reader.ReadInt32();
            Unknown12 = reader.ReadInt32();
            Unknown13 = reader.ReadInt32();

            ClanUnits = reader.ReadInt32();
            TownHall = reader.ReadInt32();
            Username = reader.ReadString();
            FacebookID = reader.ReadString();
            Stars = reader.ReadInt32();
            Points = reader.ReadInt32();

            Unknown14 = reader.ReadInt32();
            Unknown15 = reader.ReadInt32();
            Unknown16 = reader.ReadInt32();
            Unknown17 = reader.ReadInt32();

            Trophies = reader.ReadInt32();
            AttacksWon = reader.ReadInt32();
            Unknown18 = reader.ReadInt32();
            DefenseWon = reader.ReadInt32();
            Unknown19 = reader.ReadInt32();

            Unknown20 = reader.ReadInt32();
            Unknown21 = reader.ReadInt32();
            Unknown22 = reader.ReadInt32();

            bool Unknown23 = reader.ReadBoolean();
            if (Unknown23)
                Unknown24 = reader.ReadInt64();

            Unknown25 = reader.ReadByte();
            Unknown26 = reader.ReadInt32();
            Unknown27 = reader.ReadInt32();
            Unknown28 = reader.ReadInt32();
            Unknown29 = reader.ReadInt32();

            var count = reader.ReadInt32(); //storage
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //resources
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //units
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //spells
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //unit_upgrades
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //spell_upgrades
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //hero_upgrades
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //hero_health
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //hero_state
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
            }

            count = reader.ReadInt32(); //alliance_units
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var capacity = reader.ReadInt32();
                var level = reader.ReadInt32();
            }

            // more (needed?) values...
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
