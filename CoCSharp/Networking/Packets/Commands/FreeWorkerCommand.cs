namespace CoCSharp.Networking.Packets.Commands
{
    public class FreeWorkerCommand : ICommand
    {
        //TODO: Implement EmbeddedCommands

        public int ID { get { return 0x209; } }

        public int TimeLeft;
        public ICommand EmbeddedCommand;

        public void ReadCommand(PacketReader reader)
        {
            TimeLeft = reader.ReadInt32();
            //if (reader.ReadBoolean())
            //      read the thing
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(TimeLeft);
            //if(EmbeddedCommand!= null)
            //    writer the thing
        }
    }
}
