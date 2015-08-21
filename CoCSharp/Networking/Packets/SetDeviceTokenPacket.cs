namespace CoCSharp.Networking.Packets
{
    public class SetDeviceTokenPacket : IPacket
    {
        public ushort ID { get { return 0x2781; } }

        public string Token;

        public void ReadPacket(PacketReader reader)
        {
            Token = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(Token);
        }
    }
}
