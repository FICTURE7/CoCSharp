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

        public string Unknown1;

        public string OpenUDID;
        public string MacAddress;
        public string DeviceModel;
        public int LocaleKey;
        public string Language;
        public string AdvertisingGUID;
        public string OSVersion;

        public byte Unknown2;
        public string Unknown3;

        public bool IsAdvertisingTrackingEnabled;
        public string AndroidDeviceID;
        public string FacebookDistributionID;
        public string VendorGUID;
        public int Seed;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadInt64();
            UserToken = reader.ReadString();
            ClientMajorVersion = reader.ReadInt32();
            ClientContentVersion = reader.ReadInt32();
            ClientMinorVersion = reader.ReadInt32();
            FingerprintHash = reader.ReadString();

            Unknown1 = reader.ReadString();

            OpenUDID = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            LocaleKey = reader.ReadInt32();
            Language = reader.ReadString();
            AdvertisingGUID = reader.ReadString();
            OSVersion = reader.ReadString();

            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadString();

            AndroidDeviceID = reader.ReadString();
            FacebookDistributionID = reader.ReadString();
            IsAdvertisingTrackingEnabled = reader.ReadBoolean();
            VendorGUID = reader.ReadString();
            Seed = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(UserID);
            writer.WriteString(UserToken);
            writer.WriteInt32(ClientMajorVersion);
            writer.WriteInt32(ClientContentVersion);
            writer.WriteInt32(ClientMinorVersion);
            writer.WriteString(FingerprintHash);

            writer.WriteString(Unknown1);

            writer.WriteString(OpenUDID);
            writer.WriteString(MacAddress);
            writer.WriteString(DeviceModel);
            writer.WriteInt32(LocaleKey);
            writer.WriteString(Language);
            writer.WriteString(AdvertisingGUID);
            writer.WriteString(OSVersion);

            writer.WriteByte(Unknown2);
            writer.WriteString(Unknown3);

            writer.WriteString(AndroidDeviceID);
            writer.WriteString(FacebookDistributionID);
            writer.WriteBoolean(IsAdvertisingTrackingEnabled);
            writer.WriteString(VendorGUID);
            writer.WriteInt32(Seed);
        }
    }
}
