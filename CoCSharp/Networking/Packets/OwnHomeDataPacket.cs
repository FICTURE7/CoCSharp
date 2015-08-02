using CoCSharp.Logic;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Networking.Packets
{
    public class OwnHomeDataPacket : IPacket
    {
        //Not very consistant, it changes when in League, shizz changed since last update
        public ushort ID { get { return 0x5E25; } }

        public TimeSpan LastVisit;

        //public int Unknown1;

        public long TimeStamp;
        public long UserID;
        public TimeSpan ShieldDuration;

        //public long Unknown2;
        public bool Compressed;
        public Village Home;
        public bool HasClan; // could check if clan is null instead
        public Clan Clan;
        //public int Unknown4;

        //public long UserID2;
        //public long UserID3;

        //public byte Unknown5; // 00
        //public int Unknown6;  // 00000000
        //public long Unknown7; // ffffffff 00000000
        //public long Unknown8; // 00000000 00000000

        public int AllianceCastleLevel;
        public int AllianceCastleCapacity;
        public int AllianceCastleUsed;
        public int TownHallLevel;
        public string Username;
        public int FacebookID;
        public int Level;
        public int Experience;
        public int Gems;
        //public int Gems1;

        //public long Unknown9;

        public int TrophiesCount;
        public int AttackWon;
        public int AttackLost;
        public int DefenceWon;
        public int DefenceLost;

        public bool HasName;

        public void ReadPacket(PacketReader reader)
        {
            LastVisit = TimeSpan.FromSeconds(reader.ReadInt32());

            //Unknown1 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            TimeStamp = reader.ReadInt64();
            UserID = reader.ReadInt64();
            ShieldDuration = TimeSpan.FromSeconds(reader.ReadInt32());

            //Unknown2 = reader.ReadLong();
            reader.Seek(8, SeekOrigin.Current);
            Compressed = reader.ReadBoolean();
            Home = new Village();
            Home.ReadFromPacketReader(reader);

            //Unknown4 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            //UserID2 = reader.ReadLong();
            //UserID3 = reader.ReadLong();
            reader.Seek(16, SeekOrigin.Current);
            
            if ((HasClan = reader.ReadBoolean()))
            {
                Clan = new Clan()
                {
                    ID = reader.ReadInt64(),
                    Name = reader.ReadString(),
                    Badge = reader.ReadInt32(),
                };
                Level = reader.ReadInt32(); // member status?
                Level = reader.ReadInt32();
            }
            
            if (reader.ReadBoolean()) 
                reader.Seek(8, SeekOrigin.Current);
            if (reader.ReadBoolean())
                reader.Seek(8, SeekOrigin.Current);

            //Unknown5 = (byte)reader.ReadByte();
            //Unknown6 = reader.ReadInt();
            //Unknown7 = reader.ReadLong();
            //Unknown8 = reader.ReadLong();
            reader.Seek(4, SeekOrigin.Current);

            AllianceCastleLevel = reader.ReadInt32();
            AllianceCastleCapacity = reader.ReadInt32();
            AllianceCastleUsed = reader.ReadInt32();
            TownHallLevel = reader.ReadInt32();
            Username = reader.ReadString();
            FacebookID = reader.ReadInt32();
            Level = reader.ReadInt32();
            Experience = reader.ReadInt32();
            Gems = reader.ReadInt32();
            //Gems1 = reader.ReadInt();

            //Unknown9 = reader.ReadLong();
            reader.Seek(12, SeekOrigin.Current);

            TrophiesCount = reader.ReadInt32();
            AttackWon = reader.ReadInt32();
            AttackLost = reader.ReadInt32();
            DefenceWon = reader.ReadInt32();
            DefenceLost = reader.ReadInt32();

            reader.Seek(21, SeekOrigin.Current);

            HasName = reader.ReadBoolean();

            reader.Seek(16, SeekOrigin.Current);
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
