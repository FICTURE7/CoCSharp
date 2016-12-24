using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to 
    /// send commands to the client.
    /// </summary>
    public class AvailableServerCommandMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableServerCommandMessage"/> class.
        /// </summary>
        public AvailableServerCommandMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AvailableServerCommandMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24111; } }

        /// <summary>
        /// <see cref="Command"/> to send to the client.
        /// </summary>
        public Command Command;

        /// <summary>
        /// Reads the <see cref="AvailableServerCommandMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvailableServerCommandMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            var id = reader.ReadInt32();
            var command = CommandFactory.Create(id);
            if (command == null)
                return;

            command.ReadCommand(reader);
        }

        /// <summary>
        /// Writes the <see cref="AvailableServerCommandMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvailableServerCommandMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Command"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (Command == null)
                throw new InvalidOperationException("Command cannot be null.");

            writer.Write(Command.Id);
            Command.WriteCommand(writer);
        }
    }
}
