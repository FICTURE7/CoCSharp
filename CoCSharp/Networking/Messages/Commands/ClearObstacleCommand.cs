using CoCSharp.Logic;

namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that an obstacle was cleared.
    /// </summary>
    public class ClearObstacleCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="ClearObstacleCommand"/> class.
        /// </summary>
        public ClearObstacleCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="ClearObstacleCommand"/>.
        /// </summary>
        public override int ID { get { return 507; } }

        /// <summary>
        /// <see cref="Obstacle"/> game index in <see cref="Village.Obstacles"/> list that
        /// was cleared.
        /// </summary>
        public int ObstacleGameIndex;
        
        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="ClearObstacleCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ClearObstacleCommand"/>.
        /// </param>
        public override void ReadCommand(MessageReader reader)
        {
            var gameID = reader.ReadInt32();
            if (!Obstacle.ValidGameID(gameID))
                throw new InvalidCommandException("Unexpected game ID: " + gameID, this);

            ObstacleGameIndex = Obstacle.GameIDToIndex(gameID);

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="ClearObstacleCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ClearObstacleCommand"/>.
        /// </param>
        public override void WriteCommand(MessageWriter writer)
        {
            writer.Write(ObstacleGameIndex);

            writer.Write(Unknown1);
        }
    }
}
