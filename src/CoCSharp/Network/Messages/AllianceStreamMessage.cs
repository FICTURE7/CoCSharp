using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to provide the list of
    /// <see cref="AllianceStreamEntry"/>.
    /// </summary>
    public class AllianceStreamMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        public AllianceStreamMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceStreamMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24311; } }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// List of <see cref="AllianceStreamEntry"/>.
        /// </summary>
        public AllianceStreamEntry[] Entries;

        /// <summary>
        /// Reads the <see cref="AllianceStreamMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceStreamMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32();

            var count = reader.ReadInt32();
            Entries = new AllianceStreamEntry[count];
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var entry = StreamEntryFactory.CreateAllianceStreamEntry(id);
                if (entry == null)
                    return;

                entry.ReadStreamEntry(reader);
                Entries[i] = entry;
            }
        }

        /// <summary>
        /// Writes the <see cref="AllianceStreamMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceStreamMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);

            var count = Entries.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                var entry = Entries[i];
                if (entry == null)
                    throw new Exception();

                writer.Write(entry.Id);
                entry.WriteStreamEntry(writer);
            }
        }
    }
}
