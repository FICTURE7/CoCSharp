using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server
    /// to tell it that multiple buildings was upgraded. This
    /// is mostly for walls.
    /// </summary>
    public class UpgradeMultipleBuildableCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeMultipleBuildableCommand"/> class.
        /// </summary>
        public UpgradeMultipleBuildableCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="UpgradeMultipleBuildableCommand"/>.
        /// </summary>
        public override int Id { get { return 549; } }

        /// <summary>
        /// Determines whether to use the buildings alternative resources.
        /// </summary>
        public bool UseAlternativeResource;
        /// <summary>
        /// Game IDs of the buildings that was upgraded.
        /// </summary>
        public int[] BuildingsGameID;

        /// <summary>
        /// Reads the <see cref="UpgradeMultipleBuildableCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UpgradeMultipleBuildableCommand"/>.
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

            Tick = reader.ReadInt32(); // 4746
        }

        /// <summary>
        /// Writes the <see cref="UpgradeMultipleBuildableCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UpgradeMultipleBuildableCommand"/>.
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

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="UpgradeMultipleBuildableCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="UpgradeMultipleBuildableCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            for (int i = 0; i < BuildingsGameID.Length; i++)
            {
                var gameId = BuildingsGameID[i];
                var vilobj = village.VillageObjects[gameId];
                if (vilobj == null)
                {
                    level.Logs.Log($"Could not find village object with game ID {gameId}.");
                }
                else
                {
                    if (vilobj is Building)
                    {
                        var building = (Building)vilobj;
                        var data = building.NextUpgrade;
                        if (data == null)
                        {
                            var name = building.Data?.Name;
                            level.Logs.Log($"Unable to find next upgrade for building with level {building.UpgradeLevel + 1} {name}.");
                        }
                        else
                        {
                            var resource = UseAlternativeResource ? data.AltBuildResource : data.BuildResource;

                            if (building.IsConstructing)
                            {
                                level.Logs.Log($"Building at {gameId} was already in construction.");
                            }
                            else
                            {
                                level.Avatar.UseResource(resource, data.BuildCost);
                                building.BeginConstruction(Tick);
                            }
                        }
                    }
                    else
                    {
                        level.Logs.Log($"Unexpected VillageObject type: {vilobj.GetType().Name} was asked to be upgraded.");
                    }
                }
            }
        }
    }
}
