namespace CoCSharp.Networking.Packets.Commands
{
    public class BuyShieldCommand : ICommand
    {
        public int ID { get { return 0x20A; } }

        public int ShieldID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            ShieldID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(ShieldID);
            writer.WriteInt32(Unknown1);
        }
    }
}
