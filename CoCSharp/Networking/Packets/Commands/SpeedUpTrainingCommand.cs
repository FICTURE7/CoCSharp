namespace CoCSharp.Networking.Packets.Commands
{
    // rename to SpeedUpTrainUnitCommand?

    public class SpeedUpTrainingCommand : ICommand
    {
        public int ID { get { return 0x201; } }

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
