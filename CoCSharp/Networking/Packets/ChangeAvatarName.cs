namespace CoCSharp.Networking.Packets
{
    public class ChangeAvatarName : IPacket
    {
        public ushort ID { get { return 0x27E4; } }

        public string NewName;
        //public bool Unknown1;

        public void ReadPacket(PacketReader reader)
        {
            NewName = reader.ReadString();
            //Unknown1 = reader.ReadBool();
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
