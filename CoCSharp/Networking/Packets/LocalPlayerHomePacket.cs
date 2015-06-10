namespace CoCSharp.Networking.Packets
{
    public class LocalPlayerHomePacket : IPacket
    {
        public ushort ID { get { return 0x5E25; } }

        public void ReadPacket(PacketReader reader)
        {

        }

        public void WritePacket(CoCStream writer)
        {

        }
    }
}
