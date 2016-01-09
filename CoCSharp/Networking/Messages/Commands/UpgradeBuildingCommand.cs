using CoCSharp.Logic;

namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a building was upgraded.
    /// </summary>
    public class UpgradeBuildingCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="UpgradeBuildingCommand"/> class.
        /// </summary>
        public UpgradeBuildingCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="UpgradeBuildingCommand"/>.
        /// </summary>
        public override int ID { get { return 502; } }

        /// <summary>
        /// <see cref="Building"/> game index in <see cref="Village.Buildings"/> list that
        /// was upgraded.
        /// </summary>
        public int BuildingGameIndex;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;
        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;

        /// <summary>
        /// Reads the <see cref="UpgradeBuildingCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UpgradeBuildingCommand"/>.
        /// </param>
        public override void ReadCommand(MessageReader reader)
        {
            var gameID = reader.ReadInt32();
            if (!Building.ValidGameID(gameID))
                throw new InvalidCommandException("Unexpected data ID: " + gameID, this);

            BuildingGameIndex = Building.GameIDToIndex(gameID);

            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="UpgradeBuildingCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UpgradeBuildingCommand"/>.
        /// </param>
        public override void WriteCommand(MessageWriter writer)
        {
            writer.Write(BuildingGameIndex);

            writer.Write(Unknown1);
            writer.Write(Unknown2);
        }
    }
}
