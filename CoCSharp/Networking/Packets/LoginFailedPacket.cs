using CoCSharp.Data;

namespace CoCSharp.Networking.Packets
{
    public class LoginFailedPacket : IPacket
    {
        public enum LoginFailureReason : int
        {
            OutdatedContent = 7,
            OutdatedVersion = 8,
            Maintenance = 10,
            Banned = 11,
        };

        public ushort ID { get { return 0x4E87; } }

        public LoginFailureReason FailureReason;
        public Fingerprint Fingerprint;
        public string HostName;
        public string RackcdnUrl;
        public string iTunesUrl; // market url
        private string Unknown1;
        private string Unknown2;
        private byte Unknown3;
        public byte[] CompressedFingerprintJson;
        private string Unknown4;

        public void ReadPacket(PacketReader reader)
        {
            FailureReason = (LoginFailureReason)reader.ReadInt32();
            var fingerprintJson = reader.ReadString();
            if (fingerprintJson != null)
                Fingerprint = new Fingerprint(fingerprintJson);
            HostName = reader.ReadString();
            RackcdnUrl = reader.ReadString();
            iTunesUrl = reader.ReadString();
            Unknown1 = reader.ReadString();
            Unknown2 = reader.ReadString();
            Unknown3 = (byte)reader.ReadByte();
            CompressedFingerprintJson = reader.ReadByteArray();
            Unknown4 = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32((int)FailureReason);
            if (Fingerprint != null)
                writer.WriteString(Fingerprint.ToJson());
            writer.WriteString(HostName);
            writer.WriteString(RackcdnUrl);
            writer.WriteString(iTunesUrl);
            writer.WriteString(Unknown1);
            writer.WriteString(Unknown2);
            writer.WriteByte(Unknown3);
            writer.WriteByteArray(CompressedFingerprintJson);
            writer.WriteString(Unknown4);
        }
    }
}
