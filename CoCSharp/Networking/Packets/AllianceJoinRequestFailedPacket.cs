namespace CoCSharp.Networking.Packets
{
    public class AllianceJoinRequestFailedPacket : IPacket
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
