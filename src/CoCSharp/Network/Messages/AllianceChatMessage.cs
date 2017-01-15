using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to tell it
    /// that it has sent a message to the alliance chat.
    /// </summary>
    public class AllianceChatMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceChatMessage"/> class.
        /// </summary>
        public AllianceChatMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceChatMessage"/>.
        /// </summary>
        public override ushort Id => 14315;

        /// <summary>
        /// Message that should be sent to the alliance chat.
        /// </summary>
        public string MessageText;

        /// <summary>
        /// Reads the <see cref="AllianceChatMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceChatMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            MessageText = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="AllianceChatMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceChatMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(MessageText);
        }
    }
}
