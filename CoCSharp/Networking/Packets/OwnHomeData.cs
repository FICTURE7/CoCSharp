using CoCSharp.Logic;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Networking.Packets
{
    public class OwnHomeData : IPacket
    {
        public ushort ID { get { return 0x5E25; } }

        public TimeSpan LastLogged;

        //public int Unknown1;

        public long TimeStamp;
        public long UserID;
        public TimeSpan ShieldDuration;

        //public long Unknown2;
        public byte Compressed;

        //public int CompressedDataLength;
        //public int DecompressedDataLength;
        //public byte[] HomeData;
        //public string HomeJson;
        public Village Home;

        //public int Unknown4;

        //public long UserID2;
        //public long UserID3;

        //public byte Unknown5; // 00
        //public int Unknown6;  // 00000000
        //public long Unknown7; // ffffffff 00000000
        //public long Unknown8; // 00000000 00000000

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
            //Unknown3 = (byte)reader.ReadByte();
            reader.Seek(8, SeekOrigin.Current);
            Compressed = (byte)reader.ReadByte();

            //TODO: Make this moar fancy
            Home = new Village();
            Home.FromPacketReader(reader);

            //Unknown4 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            //UserID2 = reader.ReadLong();
            //UserID3 = reader.ReadLong();
            reader.Seek(16, SeekOrigin.Current);

            //Unknown5 = (byte)reader.ReadByte();
            //Unknown6 = reader.ReadInt();
            //Unknown7 = reader.ReadLong();
            //Unknown8 = reader.ReadLong();
            reader.Seek(17, SeekOrigin.Current);

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
