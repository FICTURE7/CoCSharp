namespace CoCSharp.Networking.Packets
{
    public class AllianceJoinFailedPacket : IPacket
    {
        public ushort ID { get { return 0x5EFF; } }

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
