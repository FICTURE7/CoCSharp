namespace CoCSharp.Networking.Packets
{
    public class AvatarRankListRequestPacket : IPacket
    {
        public ushort ID { get { return 0x3844; } }

        public byte Unknown1;
        public long UserID;

        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadByte();
            UserID = reader.ReadInt64();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteByte(Unknown1);
            writer.WriteInt64(UserID);
        }
    }
}
