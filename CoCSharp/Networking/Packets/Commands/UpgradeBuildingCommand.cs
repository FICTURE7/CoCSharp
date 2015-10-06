namespace CoCSharp.Networking.Packets.Commands
{
    public class UpgradeBuildingCommand : ICommand
    {
        public int ID { get { return 0x1F6; } }

        public int BuildingID;
        public byte Unknown1;
        public int Unknown2;

        public void ReadCommand(PacketReader reader)
        {
            BuildingID = reader.ReadInt32();
            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(BuildingID);
            writer.WriteByte(Unknown1);
            writer.WriteInt32(Unknown2);
        }
    }
}
