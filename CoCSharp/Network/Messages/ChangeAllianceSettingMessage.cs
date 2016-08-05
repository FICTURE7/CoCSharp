using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to tell
    /// it that it has changed alliance setting.
    /// </summary>
    public class ChangeAllianceSettingMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAllianceSettingMessage"/> class.
        /// </summary>
        public ChangeAllianceSettingMessage()
        {
            // Space
        }

        /// <summary>
        /// New alliance description.
        /// </summary>
        public string Description;


        /// <summary>
        /// Gets the ID of the <see cref="ChangeAllianceSettingMessage"/>.
        /// </summary>
        public override ushort ID { get { return 14316; } }

        /// <summary>
        /// Reads the <see cref="ChangeAllianceSettingMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChangeAllianceSettingMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Description = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="ChangeAllianceSettingMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChangeAllianceSettingMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Description);
        }
    }
}
