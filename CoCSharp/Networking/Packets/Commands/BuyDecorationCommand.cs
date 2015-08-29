namespace CoCSharp.Networking.Packets.Commands
{
    public class BuyDecorationCommand : ICommand
    {
        public int ID { get { return 0x200; } }

        public int X;
        public int Y;
        public int DecorationID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            DecorationID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(X);
            writer.WriteInt32(Y);
            writer.WriteInt32(DecorationID);
            writer.WriteInt32(Unknown1);
        }
    }
}
