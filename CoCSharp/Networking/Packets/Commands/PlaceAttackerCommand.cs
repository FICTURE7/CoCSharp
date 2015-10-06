namespace CoCSharp.Networking.Packets.Commands
{
    public class PlaceAttackerCommand : ICommand
    {
        public int ID { get { return 0x258; } }

        public int X;
        public int Y;
        public int UnitID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            UnitID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(X);
            writer.WriteInt32(Y);
            writer.WriteInt32(UnitID);
            writer.WriteInt32(Unknown1);
        }
    }
}
