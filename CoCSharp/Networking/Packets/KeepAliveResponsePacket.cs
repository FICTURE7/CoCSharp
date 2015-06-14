namespace CoCSharp.Networking.Packets
{
    public class KeepAliveResponsePacket : IPacket
    {
        public ushort ID { get { return 0x4E8C; } }

        public void ReadPacket(PacketReader reader) { } // empty packet
        public void WritePacket(PacketWriter writer) { }
    }
}
