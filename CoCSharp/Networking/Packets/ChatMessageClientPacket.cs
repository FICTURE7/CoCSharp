namespace CoCSharp.Networking.Packets
{
    public class ChatMessageClientPacket : IPacket
    {
        public ushort ID { get { return 0x397B; } }

        public string Message;

        public void ReadPacket(PacketReader reader)
        {
            Message = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(Message);
        }
    }
}
