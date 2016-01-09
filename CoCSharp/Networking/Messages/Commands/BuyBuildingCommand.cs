using CoCSharp.Logic;

namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a building was bought.
    /// </summary>
    public class BuyBuildingCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="BuyBuildingCommand"/> class.
        /// </summary>
        public BuyBuildingCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyBuildingCommand"/>.
        /// </summary>
        public override int ID { get { return 500; } }

        /// <summary>
        /// X coordinates of the building.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the building.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data index of the building that was bought.
        /// </summary>
        public int BuildingDataIndex;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="BuyBuildingCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyBuildingCommand"/>.
        /// </param>
        public override void ReadCommand(MessageReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();

            var dataID = reader.ReadInt32();
            if (!Building.ValidDataID(dataID))
                throw new InvalidCommandException("Unexpected data ID: " + dataID, this);

            BuildingDataIndex = Building.DataIDToIndex(dataID);

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyBuildingCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyBuildingCommand"/>.
        /// </param>
        public override void WriteCommand(MessageWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(BuildingDataIndex);

            writer.Write(Unknown1);
        }
    }
}
