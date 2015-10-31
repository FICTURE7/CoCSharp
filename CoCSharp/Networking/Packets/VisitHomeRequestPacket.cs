namespace CoCSharp.Networking.Packets
{
    public class VisitHomeRequestPacket : IPacket
    {
        public ushort ID { get { return 0x3721; } }

        public long UserID;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadInt64();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(UserID);
        }
    }
}