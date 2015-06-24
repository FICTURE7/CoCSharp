using CoCSharp.Logic;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Networking.Packets
{
    public class OwnHomeDataPacket : IPacket
    {
        //Not very consistant, it changes when in League
        public ushort ID { get { return 0x5E25; } }

        public TimeSpan LastLogged;

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
            LastLogged = TimeSpan.FromSeconds(reader.ReadInt());

            //Unknown1 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            TimeStamp = reader.ReadLong();
            UserID = reader.ReadLong();
            ShieldDuration = TimeSpan.FromSeconds(reader.ReadInt());

            //Unknown2 = reader.ReadLong();
            reader.Seek(8, SeekOrigin.Current);
            Compressed = reader.ReadBool();
            Home = new Village();
            Home.FromPacketReader(reader);

            //Unknown4 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            //UserID2 = reader.ReadLong();
            //UserID3 = reader.ReadLong();
            reader.Seek(16, SeekOrigin.Current);
            
            if ((HasClan = reader.ReadBool()))
            {
                Clan = new Clan()
                {
                    ID = reader.ReadLong(),
                    Name = reader.ReadString(),
                    Badge = reader.ReadInt(),
                };
                Level = reader.ReadInt(); // member status?
                Level = reader.ReadInt();
            }
            
            if (reader.ReadBool()) 
                reader.Seek(8, SeekOrigin.Current);
            if (reader.ReadBool())
                reader.Seek(8, SeekOrigin.Current);

            //Unknown5 = (byte)reader.ReadByte();
            //Unknown6 = reader.ReadInt();
            //Unknown7 = reader.ReadLong();
            //Unknown8 = reader.ReadLong();
            reader.Seek(4, SeekOrigin.Current);

            AllianceCastleLevel = reader.ReadInt();
            AllianceCastleCapacity = reader.ReadInt();
            AllianceCastleUsed = reader.ReadInt();
            TownHallLevel = reader.ReadInt();
            Username = reader.ReadString();
            FacebookID = reader.ReadInt();
            Level = reader.ReadInt();
            Experience = reader.ReadInt();
            Gems = reader.ReadInt();
            //Gems1 = reader.ReadInt();

            //Unknown9 = reader.ReadLong();
            reader.Seek(12, SeekOrigin.Current);

            TrophiesCount = reader.ReadInt();
            AttackWon = reader.ReadInt();
            AttackLost = reader.ReadInt();
            DefenceWon = reader.ReadInt();
            DefenceLost = reader.ReadInt();

            reader.Seek(21, SeekOrigin.Current);

            HasName = reader.ReadBool();

            reader.Seek(16, SeekOrigin.Current);
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
