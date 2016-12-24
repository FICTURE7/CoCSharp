using System;
using CoCSharp.Network;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell it to free a worker.
    /// </summary>
    public class FreeWorkerCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeWorkerCommand"/>.
        /// </summary>
        public FreeWorkerCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="FreeWorkerCommand"/>.
        /// </summary>
        public override int Id => 521;

        /// <summary>
        /// Embeds command.
        /// </summary>
        public bool EmbedCommand;
        /// <summary>
        /// Embedded command.
        /// </summary>
        public Command Command;

        /// <summary>
        /// Reads the <see cref="FreeWorkerCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="FreeWorkerCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <summary/>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Tick = reader.ReadInt32();
            EmbedCommand = reader.ReadBoolean();
            if (EmbedCommand)
            {
                Depth++;

                if (Depth >= MaxEmbeddedDepth)
                    throw new CommandException("A command contained embedded command depth was greater than max embedded commands.");

                var id = reader.ReadInt32();
                if (!CommandFactory.TryCreate(id, out Command))
                    return;

                Command.Depth = Depth;
                Command.ReadCommand(reader);
            }
        }

        /// <summary>
        /// Writes the <see cref="FreeWorkerCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="FreeWorkerCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Tick);
            writer.Write(EmbedCommand);
            if (EmbedCommand)
            {
                writer.Write(Command.Id);
                Command.Tick = -1;
                Command.WriteCommand(writer);
            }
        }

        public override void Execute(Level level)
        {
            level.Village.WorkerManager.FinishFastestTask(-1);

            // Execute the embedded command using on the same tick
            // as this command.
            if (EmbedCommand && Command != null)
            {
                Command.Tick = Tick;
                Command.Execute(level);
                Command.Tick = -1;
            }
        }
    }
}
