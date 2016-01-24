namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a decoration was bought.
    /// </summary>
    public class BuyDecoration : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyDecoration"/> class.
        /// </summary>
        public BuyDecoration()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyDecoration"/>.
        /// </summary>
        public override int ID { get { return 512; } }

        /// <summary>
        /// X coordinates of the decoration.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the decoration.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data index of the decoration that was bought.
        /// </summary>
        public int DecorationDataIndex;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="BuyDecoration"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyDecoration"/>.
        /// </param>
        public override void ReadCommand(MessageReader reader)
        {
            //X = reader.ReadInt32();
            //Y = reader.ReadInt32();

            //var dataID = reader.ReadInt32();
            //if (!Building.ValidDataID(dataID))
            //    throw new InvalidCommandException("Unexpected data ID: " + dataID, this);

            //BuildingDataIndex = Building.DataIDToIndex(dataID);

            //Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyDecoration"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyDecoration"/>.
        /// </param>
        public override void WriteCommand(MessageWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(DecorationDataIndex);

            writer.Write(Unknown1);
        }
    }
}
