namespace CoCSharp.Networking.Packets
{
    public class OperationPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public void ReadPacket(PacketReader reader)
        { }

        public void WritePacket(CoCStream writer)
        { }
    }
}
