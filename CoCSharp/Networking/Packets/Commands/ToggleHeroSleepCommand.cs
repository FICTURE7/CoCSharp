namespace CoCSharp.Networking.Packets.Commands
{
    public class ToggleHeroSleepCommand : ICommand
    {
        public int ID { get { return 0x211; } }

        public int BuildingID;
        public bool IsSleeping;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            BuildingID = reader.ReadInt32();
            IsSleeping = reader.ReadBoolean();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(BuildingID);
            writer.WriteBoolean(IsSleeping);
            writer.WriteInt32(Unknown1);
        }
    }
}
