namespace CoCSharp.Networking.Packets
{
    public class KeepAliveResponsePacket : IPacket
    {
        public ushort ID { get { return 0x4E8C; } }

        public void ReadPacket(PacketReader reader)
        {
            // Space
        }

        public void WritePacket(PacketWriter writer)
        {
            // Space
        }
    }
}
