namespace CoCSharp.Networking.Packets.Commands
{
    public class BoostBuildingCommand : ICommand
    {
        public int ID { get { return 0x20E; } }

        public int BuildingID;
        public int Unknown1;

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
