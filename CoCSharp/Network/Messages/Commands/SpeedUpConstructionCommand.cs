using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to
    /// tell it that a <see cref="VillageObject"/>'s construction
    /// was speed up.
    /// </summary>
    public class SpeedUpConstructionCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedUpConstructionCommand"/> class.
        /// </summary>
        public SpeedUpConstructionCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="SpeedUpConstructionCommand"/>.
        /// </summary>
        public override int ID { get { return 504; } }

        /// <summary>
        /// Game ID of the <see cref="Buildable{TCsvData}"/> whose
        /// construction was speed up.
        /// </summary>
        public int BuildableGameID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="SpeedUpConstructionCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="SpeedUpConstructionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildableGameID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="SpeedUpConstructionCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="SpeedUpConstructionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildableGameID);

            writer.Write(Unknown1);
        }
    }
}
