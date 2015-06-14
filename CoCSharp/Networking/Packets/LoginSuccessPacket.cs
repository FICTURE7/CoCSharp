using System;
using System.IO;
namespace CoCSharp.Networking.Packets
{
    public class LoginSuccessPacket : IPacket
    {
        public ushort ID { get { return 0x4E88; } }

        public long UserID;
        public string UserToken;
        public string FacebookID;
        public string GameCenterID;
        public int MajorVersion;
        public int MinorVersion;
        public int RevisionVersion;
        public string ServerEnvironment;
        public int LoginCount;
        public TimeSpan PlayTime;

        //public int Unknown1;

        public string FacebookAppID;
        public string DateLastPlayed;
        public string DateJoined;

        //public int Unknown2;

        public string GooglePlusID;
        public string CountryCode;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadLong();

            reader.Seek(8, SeekOrigin.Current);

            UserToken = reader.ReadString();
            FacebookID = reader.ReadString();
            GameCenterID = reader.ReadString();
            MajorVersion = reader.ReadInt();
            MinorVersion = reader.ReadInt();
            RevisionVersion = reader.ReadInt();
            ServerEnvironment = reader.ReadString();
            LoginCount = reader.ReadInt();
            PlayTime = TimeSpan.FromSeconds(reader.ReadInt());
            
            //Unknown1 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            FacebookAppID = reader.ReadString();
            DateLastPlayed = reader.ReadString();
            DateJoined = reader.ReadString();

            //Unknown2 = reader.ReadInt();
            reader.Seek(4, SeekOrigin.Current);

            GooglePlusID = reader.ReadString();
            CountryCode = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
