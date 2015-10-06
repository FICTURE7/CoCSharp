namespace CoCSharp.Networking.Packets.Commands
{
    public class BuyResourcesCommand : ICommand
    {
        //TODO: Implement EmbeddedCommands

        public int ID { get { return 0x206; } }

        public int ResourceID;
        public int Amount;
        public ICommand EmbeddedCommand;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            ResourceID = reader.ReadInt32();
            Amount = reader.ReadInt32();
            //if (reader.ReadBoolean())
            //    EmbeddedCommand = read the command
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(ResourceID);
            writer.WriteInt32(Amount);
            //if(EmbeddedCommand != null)
            //    write the command
            writer.WriteInt32(Unknown1);
        }
    }
}
