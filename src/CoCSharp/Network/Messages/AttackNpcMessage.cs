using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to server to tell
    /// it that an NPC village is being attacked.
    /// </summary>
    public class AttackNpcMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDataMessage"/> class.
        /// </summary>
        public AttackNpcMessage()
        {
            // Space
        }

        /// <summary>
        ///  Gets the ID of the <see cref="AttackNpcMessage"/>.
        /// </summary>
        public override ushort ID { get { return 14134; } }

        /// <summary>
        /// NPC ID that was attacked.
        /// </summary>
        public int NpcID;

        /// <summary>
        /// Reads the <see cref="AttackNpcMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AttackNpcMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            NpcID = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AttackNpcMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AttackNpcMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(NpcID);
        }
    }
}
