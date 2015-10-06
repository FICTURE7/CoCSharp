namespace CoCSharp.Networking.Packets.Commands
{
    public class UpgradeUnitCommand : ICommand
    {
        public int ID { get { return 0x204; } }

        public int BuildingID;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;

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
