namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to server to tell
    /// it that an attack was finished; the server then should
    /// send an <see cref="OwnHomeDataMessage"/>.
    /// </summary>
    public class AttackResultMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttackResultMessage"/> class.
        /// </summary>
        public AttackResultMessage()
        {
            // Space
        }

        /// <summary>
        /// Unknown long 1.
        /// </summary>
        public long Unknown1;

        /// <summary>
        /// Gets the ID of the <see cref="AttackResultMessage"/>.
        /// </summary>
        public override ushort ID { get { return 14101; } }

        /// <summary>
        /// Reads the <see cref="AttackResultMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AttackResultMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            Unknown1 = reader.ReadInt64();
        }

        /// <summary>
        /// Writes the <see cref="AttackResultMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AttackResultMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            writer.Write(Unknown1);
        }
    }
}
