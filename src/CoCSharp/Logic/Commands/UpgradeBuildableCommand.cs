using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a building was upgraded.
    /// </summary>
    public class UpgradeBuildableCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeBuildableCommand"/> class.
        /// </summary>
        public UpgradeBuildableCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="UpgradeBuildableCommand"/>.
        /// </summary>
        public override int ID { get { return 502; } }

        /// <summary>
        /// Game ID of the <see cref="Buildable{TCsvData}"/> that was upgraded.
        /// </summary>
        public int BuildableGameID;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1; // UseAlternativeResource?

        /// <summary>
        /// Reads the <see cref="UpgradeBuildableCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UpgradeBuildableCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildableGameID = reader.ReadInt32();

            Unknown1 = reader.ReadByte();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="UpgradeBuildableCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UpgradeBuildableCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildableGameID);

            writer.Write(Unknown1);

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="UpgradeBuildableCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="UpgradeBuildableCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            var vilobj = village.VillageObjects[BuildableGameID];
            if (vilobj == null)
            {
                level.Logs.Log($"Could not find village object with game ID {BuildableGameID}.");
            }
            else
            {
                //TODO: Check for alternative resources as well.

                if (vilobj is Building)
                {
                    var building = (Building)vilobj;
                    var data = building.NextUpgrade;
                    if (building.IsConstructing)
                    {
                        level.Logs.Log($"Building at {BuildableGameID} was already in construction.");
                    }
                    else
                    {
                        level.Avatar.UseResource(data.BuildResource, data.BuildCost);
                        building.BeginConstruction(Tick);
                    }
                }
                else if (vilobj is Trap)
                {
                    var trap = (Trap)vilobj;
                    var data = trap.NextUpgrade;
                    if (trap.IsConstructing)
                    {
                        level.Logs.Log($"Trap at {BuildableGameID} was already in construction.");
                    }
                    else
                    {
                        level.Avatar.UseResource(data.BuildResource, data.BuildCost);
                        trap.BeginConstruction(Tick);
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
