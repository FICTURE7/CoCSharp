using System;

namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server
    /// to tell it that multiple buildings was upgraded. This
    /// is mostly for walls.
    /// </summary>
    public class UpgradeMultipleBuildingsCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="UpgradeMultipleBuildingsCommand"/> class.
        /// </summary>
        public UpgradeMultipleBuildingsCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="UpgradeMultipleBuildingsCommand"/>.
        /// </summary>
        public override int ID { get { return 549; } }

        /// <summary>
        /// Determines whether to use the buildings alternative resources.
        /// </summary>
        public bool UseAlternativeResource;
        /// <summary>
        /// Game IDs of the buildings that was upgraded.
        /// </summary>
        public int[] BuildingsGameID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="UpgradeMultipleBuildingsCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UpgradeMultipleBuildingsCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <exception cref="InvalidCommandException">Length <see cref="BuildingsGameID"/> is less 0.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UseAlternativeResource = reader.ReadBoolean();

            var count = reader.ReadInt32();
            if (count < 0)
                throw new InvalidCommandException("Length of BuildingsGameID cannot be less than 0.", this);

            BuildingsGameID = new int[count];
            for (int i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                BuildingsGameID[i] = id;
            }

            Unknown1 = reader.ReadInt32(); // 4746
        }

        /// <summary>
        /// Writes the <see cref="UpgradeMultipleBuildingsCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UpgradeMultipleBuildingsCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="BuildingsGameID"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (BuildingsGameID == null)
                throw new InvalidOperationException("BuildingsGameID cannot be null.");

            writer.Write(UseAlternativeResource);
            writer.Write(BuildingsGameID.Length);

            for (int i = 0; i < BuildingsGameID.Length; i++)
                writer.Write(BuildingsGameID[i]);

            writer.Write(Unknown1);
        }
    }
}
