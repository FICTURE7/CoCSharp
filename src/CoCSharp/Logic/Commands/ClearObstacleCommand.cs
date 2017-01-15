using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
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
        public override int Id { get { return 507; } }

        /// <summary>
        /// Game ID of the <see cref="Obstacle"/> that was cleared.
        /// </summary>
        public int ObstacleGameId;

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

            ObstacleGameId = reader.ReadInt32();

            Tick = reader.ReadInt32();
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

            writer.Write(ObstacleGameId);

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="ClearObstacleCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="ClearObstacleCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            var vilobj = village.VillageObjects[ObstacleGameId];
            if (vilobj == null)
            {
                level.Logs.Log($"Could not find village object with game ID {ObstacleGameId}.");
            }
            else
            {
                var obstacle = (Obstacle)vilobj;
                var data = obstacle.Data;
                if (obstacle.IsClearing)
                {
                    level.Logs.Log($"Tried to clear an obstacle which is already being cleared with game ID {ObstacleGameId}.");
                }
                else
                {
                    level.Avatar.UseResource(data.ClearResource, data.ClearCost);
                    obstacle.BeginClearing(Tick);
                }
            }
        }
    }
}
