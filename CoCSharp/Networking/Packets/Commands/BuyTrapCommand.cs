namespace CoCSharp.Networking.Packets.Commands
{
    public class BuyTrapCommand : ICommand
    {
        public int ID { get { return 0x1FE; } }

        public int X;
        public int Y;
        public int TrapID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            TrapID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(X);
            writer.WriteInt32(Y);
            writer.WriteInt32(TrapID);
            writer.WriteInt32(Unknown1);
        }
    }
}
