namespace CoCSharp.Networking.Packets
{
    public class KeepAliveResponsePacket : IPacket
    {
        public ushort ID { get { return 0x4E8C; } }

        public void ReadPacket(PacketReader reader) { }
        public void WritePacket(CoCStream stream) { }
    }
}
