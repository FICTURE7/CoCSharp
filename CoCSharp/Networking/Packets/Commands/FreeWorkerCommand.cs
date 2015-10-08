namespace CoCSharp.Networking.Packets.Commands
{
    public class FreeWorkerCommand : ICommand
    {
        public int ID { get { return 0x209; } }

        public int TimeLeft;
        public ICommand EmbeddedCommand;

        public void ReadCommand(PacketReader reader)
        {
            TimeLeft = reader.ReadInt32();
            if (reader.ReadBoolean())
            {
                var id = reader.ReadInt32();
                CommandFactory.TryCreate(id, out EmbeddedCommand);
            }
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(TimeLeft);
            if (EmbeddedCommand != null)
                EmbeddedCommand.WriteCommand(writer);
        }
    }
}
