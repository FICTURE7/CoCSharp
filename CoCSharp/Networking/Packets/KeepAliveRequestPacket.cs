namespace CoCSharp.Networking.Packets
{
    public class KeepAliveRequestPacket : IPacket
    {
        public ushort ID { get { return 0x277C; } }

        public void ReadPacket(PacketReader reader) { }
        public void WritePacket(CoCStream stream) { }
    }
}
