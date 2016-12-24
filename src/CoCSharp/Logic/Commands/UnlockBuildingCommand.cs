using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that it has unlocked a locked building. E.g: Clan Castle.
    /// </summary>
    public class UnlockBuildingCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlockBuildingCommand"/> class.
        /// </summary>
        public UnlockBuildingCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="UnlockBuildingCommand"/>.
        /// </summary>
        public override int Id { get { return 520; } }

        /// <summary>
        /// Game ID of the <see cref="Building"/> that was unlocked.
        /// </summary>
        public int BuildingGameID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // -1

        /// <summary>
        /// Reads the <see cref="UnlockBuildingCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UnlockBuildingCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildingGameID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyResourcesCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyResourcesCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildingGameID);

            writer.Write(Unknown1);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var vilobj = level.Village.VillageObjects[BuildingGameID];
            if (vilobj == null)
            {
                level.Logs.Log($"Could not find village object with game ID {BuildingGameID}.");
            }
            else
            {
                if (vilobj is Building)
                {
                    var building = (Building)vilobj;
                    var data = building.Data;

                    level.Avatar.UseResource(data.BuildResource, data.BuildCost);
                    building.IsLocked = false;
                }
                else
                {
                    level.Logs.Log($"Unexpected VillageObject type: {vilobj.GetType().Name} was asked to be unlocked.");
                }
            }
        }
    }
}
