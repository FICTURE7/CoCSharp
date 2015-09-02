using System;

namespace CoCSharp.Networking.Packets
{
    public class LoginSuccessPacket : IPacket
    {
        public ushort ID { get { return 0x4E88; } }

        public long UserID;
        public long UserID1;
        public string UserToken;
        public string FacebookID;
        public string GameCenterID;
        public int MajorVersion;
        public int MinorVersion;
        public int RevisionVersion;
        public string ServerEnvironment; // could implment Enum here
        public int LoginCount;
        public TimeSpan PlayTime;
        public int Unknown1;
        public string FacebookAppID;
        public DateTime DateLastPlayed;
        public DateTime DateJoined;
        public int Unknown2;
        public string GooglePlusID;
        public string CountryCode;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadInt64();
            UserID1 = reader.ReadInt64();
            UserToken = reader.ReadString();
            FacebookID = reader.ReadString();
            GameCenterID = reader.ReadString();
            MajorVersion = reader.ReadInt32();
            MinorVersion = reader.ReadInt32();
            RevisionVersion = reader.ReadInt32();
            ServerEnvironment = reader.ReadString();
            LoginCount = reader.ReadInt32();
            PlayTime = TimeSpan.FromSeconds(reader.ReadInt32());
            
            Unknown1 = reader.ReadInt32();

            FacebookAppID = reader.ReadString();
            DateLastPlayed = DateTimeConverter.FromJavaTimestamp(double.Parse(reader.ReadString()));
            DateJoined = DateTimeConverter.FromJavaTimestamp(double.Parse(reader.ReadString()));

            Unknown2 = reader.ReadInt32();

            GooglePlusID = reader.ReadString();
            CountryCode = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(UserID);
            writer.WriteInt64(UserID1);
            writer.WriteString(UserToken);
            writer.WriteString(FacebookID);
            writer.WriteString(GameCenterID);
            writer.WriteInt32(MajorVersion);
            writer.WriteInt32(MinorVersion);
            writer.WriteInt32(RevisionVersion);
            writer.WriteString(ServerEnvironment);
            writer.WriteInt32(LoginCount);
            writer.WriteInt32((int)PlayTime.TotalSeconds);

            writer.WriteInt32(Unknown1);

            writer.WriteString(FacebookAppID);
            writer.WriteString(DateTimeConverter.ToJavaTimestamp(DateLastPlayed).ToString()); // should round stuff?
            writer.WriteString(DateTimeConverter.ToJavaTimestamp(DateJoined).ToString());

            writer.WriteInt32(Unknown2);

            writer.WriteString(GooglePlusID);
            writer.WriteString(CountryCode);
        }
    }
}
