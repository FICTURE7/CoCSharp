using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that an obstacle was cleared.
    /// </summary>
    public class ClearObstacleCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearObstacleCommand"/> class.
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
        /// Game ID of the <see cref="Obstacle"/> that was cleared.
        /// </summary>
        public int ObstacleGameID;
        
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
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ObstacleGameID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="ClearObstacleCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ClearObstacleCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ObstacleGameID);

            writer.Write(Unknown1);
        }
    }
}
