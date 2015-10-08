namespace CoCSharp.Networking.Packets.Commands
{
    public class BuyResourcesCommand : ICommand
    {
        public int ID { get { return 0x206; } }

        public int ResourceID;
        public int Amount;
        public ICommand EmbeddedCommand;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            ResourceID = reader.ReadInt32();
            Amount = reader.ReadInt32();
            if (reader.ReadBoolean())
            {
                var id = reader.ReadInt32();
                CommandFactory.TryCreate(id, out EmbeddedCommand);
            }
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(ResourceID);
            writer.WriteInt32(Amount);
            if (EmbeddedCommand != null)
                EmbeddedCommand.WriteCommand(writer);
            writer.WriteInt32(Unknown1);
        }
    }
}
