namespace CoCSharp.Networking.Packets.Commands
{
    public class MissionProgressCommand : ICommand
    {
        public int ID { get { return 0x207; } }

        public int MissionID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            MissionID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(MissionID);
            writer.WriteInt32(Unknown1);
        }
    }
}
