using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a building was upgraded.
    /// </summary>
    public class UpgradeBuildableCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeBuildableCommand"/> class.
        /// </summary>
        public UpgradeBuildableCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="UpgradeBuildableCommand"/>.
        /// </summary>
        public override int ID { get { return 502; } }

        /// <summary>
        /// Game ID of the <see cref="Buildable{TCsvData}"/> that was upgraded.
        /// </summary>
        public int BuildableGameID;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;
        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;

        /// <summary>
        /// Reads the <see cref="UpgradeBuildableCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UpgradeBuildableCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildableGameID = reader.ReadInt32();

            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="UpgradeBuildableCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UpgradeBuildableCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildableGameID);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
        }
    }
}
