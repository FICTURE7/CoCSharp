using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server
    /// for it to handle in game actions.
    /// </summary>
    public class CommandMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMessage"/> class.
        /// </summary>
        public CommandMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="CommandMessage"/>.
        /// </summary>
        public override ushort ID { get { return 14102; } }

        /// <summary>
        /// Tick value.
        /// </summary>
        public int Tick;
        /// <summary>
        /// Checksum to check if the <see cref="CommandMessage"/> is valid.
        /// </summary>
        public int Checksum;
        /// <summary>
        /// Embedded commands in the <see cref="CommandMessage"/>.
        /// </summary>
        public Command[] Commands;

        /// <summary>
        /// Reads the <see cref="CommandMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="CommandMessage"/>.
        /// </param>
        /// <exception cref="InvalidMessageException">Number of embedded commands is less than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Tick = reader.ReadInt32();
            Checksum = reader.ReadInt32();

            var length = reader.ReadInt32();
            if (length < 0)
                throw new InvalidMessageException("Number of commands cannot be less than 0.");

            Commands = new Command[length];

            for (int i = 0; i < length; i++)
            {
                var cmd = (Command)null;
                var cmdID = reader.ReadInt32();
                if (!CommandFactory.TryCreate(cmdID, out cmd))
                    break; // Breaking early because we don't want to mess the stream up.

                cmd.ReadCommand(reader);
                Commands[i] = cmd;
            }
        }

        /// <summary>
        /// Writes the <see cref="CommandMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="CommandMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Tick);
            writer.Write(Checksum);

            if (Commands == null || Commands.Length == 0)
            {
                writer.Write(0);
                return;
            }

            writer.Write(Commands.Length);
            for (int i = 0; i < Commands.Length; i++)
            {
                var cmd = Commands[i];
                writer.Write(cmd.ID);
                cmd.WriteCommand(writer);
            }
        }
    }
}
