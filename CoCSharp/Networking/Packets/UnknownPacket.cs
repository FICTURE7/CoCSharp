namespace CoCSharp.Networking.Packets
{
    /// <summary>
    /// Represents an UnknownPacket or unimplemented packet.
    /// The <see cref="UnknownPacket"/> contains both the 
    /// encrypted byte array and the decrypted byte array.
    /// </summary>
    public class UnknownPacket : IPacket
    {
        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public byte[] EncryptedData;
        public byte[] DecryptedData;

        public void ReadPacket(PacketReader reader)
        {
            DecryptedData = new byte[Length];
            reader.Read(DecryptedData, 0, Length);
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.Write(EncryptedData, 0, EncryptedData.Length);
        }
    }
}
