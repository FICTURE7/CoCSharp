namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a building was moved.
    /// </summary>
    public class MoveBuildingCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="MoveBuildingCommand"/> class.
        /// </summary>
        public MoveBuildingCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="MoveBuildingCommand"/>.
        /// </summary>
        public override int ID { get { return 501; } }

        /// <summary>
        /// X coordinates of the new position.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the new position.
        /// </summary>
        public int Y;
        /// <summary>
        /// ID of the building that was moved.
        /// </summary>
        public int BuildingID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="MoveBuildingCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="MoveBuildingCommand"/>.
        /// </param>
        public override void ReadCommand(MessageReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            BuildingID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="MoveBuildingCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="MoveBuildingCommand"/>.
        /// </param>
        public override void WriteCommand(MessageWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(BuildingID);

            writer.Write(Unknown1);
        }
    }
}
