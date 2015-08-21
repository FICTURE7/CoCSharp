namespace CoCSharp.Networking.Commands
{
    public class TrainUnitCommand : ICommand
    {
        public int ID { get { return 0x1FC; } }

        public int BuildingID;
        private int Unknown1;
        public int CharacterID;
        public int Count;
        private int Unknown2;

        public void ReadCommand(PacketReader reader)
        {
            BuildingID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            CharacterID = reader.ReadInt32();
            Count = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(BuildingID);
            writer.WriteInt32(Unknown1);
            writer.WriteInt32(CharacterID);
            writer.WriteInt32(Count);
            writer.WriteInt32(Unknown2);
        }
    }
}
