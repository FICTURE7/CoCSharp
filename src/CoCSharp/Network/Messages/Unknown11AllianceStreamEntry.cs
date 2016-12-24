namespace CoCSharp.Network.Messages
{
    public class Unknown11AllianceStreamEntry : AllianceStreamEntry
    {
        public Unknown11AllianceStreamEntry()
        {
            // Space
        }

        public override int Id => 11;

        public string UnknownJson;

        public override void ReadStreamEntry(MessageReader reader)
        {
            base.ReadStreamEntry(reader);

            UnknownJson = reader.ReadString();
            var k = reader.ReadInt32();
            var d = reader.ReadByte();
            var k1 = reader.ReadInt32();
            if (d == 0)
            {
                var d1 = reader.ReadByte();
            }
            var d2 = reader.ReadByte();
            var k2 = reader.ReadInt32();
        }

        public override void WriteStreamEntry(MessageWriter writer)
        {
            base.WriteStreamEntry(writer);
        }
    }
}
