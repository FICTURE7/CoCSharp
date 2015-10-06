namespace CoCSharp.Networking.Packets.Commands
{
    public class SpeedUpClearingCommand : ICommand
    {
        public int ID { get { return 0x202; } }

        public int ObstacleID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            ObstacleID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(ObstacleID);
            writer.WriteInt32(Unknown1);
        }
    }
}
