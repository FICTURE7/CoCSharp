using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Description of AvatarStreamMessage.
    /// </summary>
    public class AvatarStreamMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarStreamMessage"/> class.
        /// </summary>
        public AvatarStreamMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AvatarStreamMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24411; } }

        /// <summary>
        /// List of <see cref="AvatarStreamEntry"/>.
        /// </summary>
        public AvatarStreamEntry[] Entries;

        /// <summary>
        /// Reads the <see cref="AvatarStreamMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarStreamMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            var count = reader.ReadInt32();
            Entries = new AvatarStreamEntry[count];
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var entry = StreamEntryFactory.CreateAvatarStreamEntry(id);
                if (entry == null)
                    return;

                entry.ReadStreamEntry(reader);
                Entries[i] = entry;
            }
        }

        /// <summary>
        /// Writes the <see cref="AvatarStreamMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarStreamMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

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
