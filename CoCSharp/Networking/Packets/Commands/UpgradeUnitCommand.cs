namespace CoCSharp.Networking.Packets.Commands
{
    public class UpgradeUnitCommand : ICommand
    {
        public int ID { get { return 0x204; } }

        public int BuildingID;
        private int Unknown1;
        private int Unknown2;
        private int Unknown3;

        public void ReadCommand(PacketReader reader)
        {
            BuildingID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(BuildingID);
            writer.WriteInt32(Unknown1);
            writer.WriteInt32(Unknown2);
            writer.WriteInt32(Unknown3);
        }
    }
}
