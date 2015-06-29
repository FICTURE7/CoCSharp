namespace CoCSharp.Networking.Packets
{
    public class ChangeAvatarNamePacket : IPacket
    {
        public ushort ID { get { return 0x27E4; } }

        public string NewName;
        private bool Unknown1;

        public void ReadPacket(PacketReader reader)
        {
            NewName = reader.ReadString();
            Unknown1 = reader.ReadBool();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(NewName);
            writer.WriteBool(Unknown1);
        }
    }
}
