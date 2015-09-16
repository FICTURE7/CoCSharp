namespace CoCSharp.Networking.Packets
{
    public class UpdateKeyPacket : IPacket
    {
        public ushort ID { get { return 0x4E20;} }

        public byte[] Key;
        public int ScramblerVersion; // = 1 Encryption version?

        public void ReadPacket(PacketReader reader)
        {
            Key = reader.ReadByteArray();
            ScramblerVersion = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteByteArray(Key);
            writer.WriteInt32(ScramblerVersion);
        }
    }
}
