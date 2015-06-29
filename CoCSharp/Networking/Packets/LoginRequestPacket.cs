using System.IO;

namespace CoCSharp.Networking.Packets
{
    public class LoginRequestPacket : IPacket
    {
        public ushort ID { get { return 0x2775; } }

        public long UserID;
        public string UserToken;
        public int ClientMajorVersion;
        public int ClientContentVersion;
        public int ClientMinorVersion;
        public string FingerprintHash;

        private string Unknown1;

        public string OpenUDID;
        public string MacAddress;
        public string DeviceModel;
        public int LocaleKey;
        public string Language;
        public string AdvertisingGUID;
        public string OsVersion;

        private byte Unknown2;
        private string Unknown3;

        public bool IsAdvertisingTrackingEnabled;
        public string AndroidDeviceID;
        public string FacebookDistributionID;
        public string VendorGUID;
        public int Seed;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadLong();
            UserToken = reader.ReadString();
            ClientMajorVersion = reader.ReadInt();
            ClientContentVersion = reader.ReadInt();
            ClientMinorVersion = reader.ReadInt();
            FingerprintHash = reader.ReadString();

            Unknown1 = reader.ReadString();

            OpenUDID = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            LocaleKey = reader.ReadInt();
            Language = reader.ReadString();
            AdvertisingGUID = reader.ReadString();
            OsVersion = reader.ReadString();

            Unknown2 = (byte)reader.ReadByte();
            Unknown3 = reader.ReadString();

            AndroidDeviceID = reader.ReadString();
            FacebookDistributionID = reader.ReadString();
            IsAdvertisingTrackingEnabled = reader.ReadBool();
            VendorGUID = reader.ReadString();
            Seed = reader.ReadInt();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteLong(UserID);
            writer.WriteString(UserToken);
            writer.WriteInt(ClientMajorVersion);
            writer.WriteInt(ClientContentVersion);
            writer.WriteInt(ClientMinorVersion);
            writer.WriteString(FingerprintHash);

            writer.WriteString(Unknown1);

            writer.WriteString(OpenUDID);
            writer.WriteString(MacAddress);
            writer.WriteString(DeviceModel);
            writer.WriteInt(LocaleKey);
            writer.WriteString(Language);
            writer.WriteString(AdvertisingGUID);
            writer.WriteString(OsVersion);

            writer.WriteByte(Unknown2);
            writer.WriteString(Unknown3);

            writer.WriteString(AndroidDeviceID);
            writer.WriteString(FacebookDistributionID);
            writer.WriteBool(IsAdvertisingTrackingEnabled);
            writer.WriteString(VendorGUID);
            writer.WriteInt(Seed);
        }
    }
}
