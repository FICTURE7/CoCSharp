namespace CoCSharp.Networking.Packets
{
    public class AllianceChatMessageClientPacket : IPacket
    {
        public ushort ID { get { return 0x37EB; } } 

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
