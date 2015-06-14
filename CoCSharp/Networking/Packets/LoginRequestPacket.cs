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

        //public string Unknown1;

        public string OpenUDID;
        public string MacAddress;
        public string DeviceModel;
        public int LocaleKey;
        public string Language;
        public string AdvertisingGUID;
        public string OsVersion;

        //public byte Unknown2;
        //public string Unknown3;

        public string AndroidDeviceID;
        public string FacebookDistributionID;
        //public bool AdvertisingTrackerEnabled; // causes buffer overflow cause of invalid VendorGUID string length
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

            /*Unknown1 = */ reader.ReadString();

            OpenUDID = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            LocaleKey = reader.ReadInt();

            Language = reader.ReadString();
            AdvertisingGUID = reader.ReadString();
            OsVersion = reader.ReadString();

            reader.Seek(1, SeekOrigin.Current);
            //Unknown2 = (byte)reader.ReadByte();
            /*Unknown3 = */ reader.ReadString();

            AndroidDeviceID = reader.ReadString();
            FacebookDistributionID = reader.ReadString();

            reader.Seek(1, SeekOrigin.Current);

            //AdvertisingTrackerEnabled = reader.ReadBool();
            VendorGUID = reader.ReadString();
            Seed = reader.ReadInt();
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
