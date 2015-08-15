namespace CoCSharp.Networking.Commands
{
    public class CancelConstructionCommand : ICommand
    {
        public int ID { get { return 0x1F9; } }

        public int BuildingID;
        private int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            BuildingID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(BuildingID);
            writer.WriteInt32(Unknown1);
        }
    }
}
