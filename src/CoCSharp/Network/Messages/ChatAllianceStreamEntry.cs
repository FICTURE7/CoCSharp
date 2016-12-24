using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Stream entry indicating chat messages sent in
    /// the alliance chat.
    /// </summary>
    public class ChatAllianceStreamEntry : AllianceStreamEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatAllianceStreamEntry"/> class.
        /// </summary>
        public ChatAllianceStreamEntry()
        {
            //space
        }

        /// <summary>
        /// Message that was sent.
        /// </summary>
        public string MessageText;

        /// <summary>
        /// Get the ID of the <see cref="ChatAllianceStreamEntry"/>
        /// </summary>
        public override int Id => 2;

        /// <summary>
        /// Reads the <see cref="ChatAllianceStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChatAllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            base.ReadStreamEntry(reader);

            MessageText = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="ChatAllianceStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChatAllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            base.WriteStreamEntry(writer);

            writer.Write(MessageText);
        }
    }
}
