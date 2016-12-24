using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to tell it
    /// that a <see cref="AllianceStreamEntry"/> should be added to the entry list.
    /// </summary>
    public class AllianceStreamEventMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEventMessage"/> class.
        /// </summary>
        public AllianceStreamEventMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceStreamEventMessage"/>.
        /// </summary>
        public override ushort Id => 24312;

        /// <summary>
        /// Entry that should be handled.
        /// </summary>
        public AllianceStreamEntry Entry;

        /// <summary>
        /// Reads the <see cref="AllianceStreamEventMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceStreamEventMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            var id = reader.ReadInt32();
            var entry = StreamEntryFactory.CreateAllianceStreamEntry(id);
            entry?.ReadStreamEntry(reader);
        }

        /// <summary>
        /// Writes the <see cref="AllianceStreamEventMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceStreamEventMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (Entry == null)
                throw new InvalidOperationException();

            writer.Write(Entry.Id);
            Entry.WriteStreamEntry(writer);
        }
    }
}
