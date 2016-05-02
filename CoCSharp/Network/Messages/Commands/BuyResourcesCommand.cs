using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that resources was bought.
    /// </summary>
    public class BuyResourcesCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyResourcesCommand"/> class.
        /// </summary>
        public BuyResourcesCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyResourcesCommand"/>.
        /// </summary>
        public override int ID { get { return 518; } }

        /// <summary>
        /// Data ID of the resource that was bought.
        /// </summary>
        public int ResourceDataID;
        /// <summary>
        /// Amount of resources bought.
        /// </summary>
        public int ResourceAmount;
        /// <summary>
        /// If it embeds another command.
        /// </summary>
        public bool EmbedCommand;
        /// <summary>
        /// <see cref="Network.Command"/> that was embedded inside of the
        /// <see cref="BuyResourcesCommand"/>.
        /// </summary>
        public Command Command;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="BuyResourcesCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyResourcesCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ResourceDataID = reader.ReadInt32();
            ResourceAmount = reader.ReadInt32();
            EmbedCommand = reader.ReadBoolean();
            if (EmbedCommand)
            {
                Depth++;
                //Console.WriteLine("Depth: {0}", Depth);

                if (Depth >= MaxEmbeddedDepth)
                    throw new CommandException("A command contained embedded command depth was greater than max embedded commands.");

                var id = reader.ReadInt32();
                if (!CommandFactory.TryCreate(id, out Command))
                    return;

                Command.Depth = Depth;
                Command.ReadCommand(reader);
            }

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyResourcesCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyResourcesCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (EmbedCommand && Command == null)
                throw new InvalidOperationException("Embedded command specified return null.");

            writer.Write(ResourceDataID);
            writer.Write(ResourceAmount);
            writer.Write(EmbedCommand);
            if (EmbedCommand)
            {
                writer.Write(Command.ID);
                Command.WriteCommand(writer);
            }

            writer.Write(Unknown1);
        }
    }
}
