namespace CoCSharp.Networking.Packets
{
    public class ServerErrorPacket : IPacket
    {
        public ushort ID { get { return 0x5E33; } }

        public string ErrorMessage;

        public void ReadPacket(PacketReader reader)
        {
            ErrorMessage = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(ErrorMessage);
        }
    }
}
