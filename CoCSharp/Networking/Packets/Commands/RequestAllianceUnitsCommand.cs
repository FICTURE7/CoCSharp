namespace CoCSharp.Networking.Packets.Commands
{
    public class RequestAllianceUnitsCommand : ICommand
    {
        public int ID { get { return 0x1FF; } }

        public int Unknown1;
        public bool HashRequestMessage;
        public string Message;

        public void ReadCommand(PacketReader reader)
        {
            Unknown1 = reader.ReadInt32();
            if ((HashRequestMessage = reader.ReadBoolean()))
                Message = reader.ReadString();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(Unknown1);
            writer.WriteBoolean(HashRequestMessage);
            if (HashRequestMessage)
                writer.WriteString(Message);
        }
    }
}
