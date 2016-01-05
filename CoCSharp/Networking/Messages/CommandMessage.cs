namespace CoCSharp.Networking.Messages
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
        /// Subtick. 1/60 secounds since logged in.
        /// </summary>
        public int Subtick;
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
        public override void ReadMessage(MessageReader reader)
        {
            Subtick = reader.ReadInt32();
            Checksum = reader.ReadInt32();

            var length = reader.ReadInt32();
            if (length < 0)
                throw new InvalidMessageException("Number of embedded commands cannot be less than 0.");

            Commands = new Command[length];

            for (int i = 0; i < length; i++)
            {
                var cmd = (Command)null;
                var cmdID = reader.ReadInt32();
                if (!CommandFactory.TryCreate(cmdID, out cmd))
                    break; // just not to mess the stream up

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
        public override void WriteMessage(MessageWriter writer)
        {
            writer.Write(Subtick);
            writer.Write(Checksum);

            if (Commands == null)
            {
                writer.Write(0);
                return; // exit early because there is no commands
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
