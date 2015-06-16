using System;
using System.IO;
namespace CoCSharp.Networking.Packets
{
    public class LoginSuccessPacket : IPacket
    {
        public ushort ID { get { return 0x4E88; } }

        public long UserID;
        private long UserID2;
        public string UserToken;
        public string FacebookID;
        public string GameCenterID;
        public int MajorVersion;
        public int MinorVersion;
        public int RevisionVersion;
        public string ServerEnvironment; // could implment Enum here
        public int LoginCount;
        public TimeSpan PlayTime;
        private int Unknown1;
        public string FacebookAppID;
        public string DateLastPlayed;
        public string DateJoined;
        private int Unknown2;
        public string GooglePlusID;
        public string CountryCode;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadLong();
            UserID2 = reader.ReadLong();
            UserToken = reader.ReadString();
            FacebookID = reader.ReadString();
            GameCenterID = reader.ReadString();
            MajorVersion = reader.ReadInt();
            MinorVersion = reader.ReadInt();
            RevisionVersion = reader.ReadInt();
            ServerEnvironment = reader.ReadString();
            LoginCount = reader.ReadInt();
            PlayTime = TimeSpan.FromSeconds(reader.ReadInt());
            
            Unknown1 = reader.ReadInt();

            FacebookAppID = reader.ReadString();
            DateLastPlayed = reader.ReadString();
            DateJoined = reader.ReadString();

            Unknown2 = reader.ReadInt();

            GooglePlusID = reader.ReadString();
            CountryCode = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
