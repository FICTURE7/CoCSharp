using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to
    /// tell it that a <see cref="VillageObject"/>'s construction
    /// was speed up.
    /// </summary>
    public class SpeedUpConstructionCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedUpConstructionCommand"/> class.
        /// </summary>
        public SpeedUpConstructionCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="SpeedUpConstructionCommand"/>.
        /// </summary>
        public override int Id { get { return 504; } }

        /// <summary>
        /// Game ID of the <see cref="Buildable{TCsvData}"/> whose
        /// construction was speed up.
        /// </summary>
        public int BuildableGameID;

        /// <summary>
        /// Reads the <see cref="SpeedUpConstructionCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="SpeedUpConstructionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildableGameID = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="SpeedUpConstructionCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="SpeedUpConstructionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildableGameID);

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="SpeedUpConstructionCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="SpeedUpConstructionCommand"/>.</param>
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
                if (vilobj is Building)
                {
                    var building = (Building)vilobj;
                    if (!building.IsConstructing)
                    {
                        level.Logs.Log($"Tried to speed the construction of a building which is not in construction with game ID {BuildableGameID}.");
                    }
                    else
                    {
                        var speedUpCost = LogicUtils.CalculateSpeedUpCost(level.Assets, building.ConstructionDuration);
                        level.Avatar.Gems -= speedUpCost;

                        building.SpeedUpConstruction(Tick);
                    }
                }
                else if (vilobj is Trap)
                {
                    var trap = (Trap)vilobj;
                    if (!trap.IsConstructing)
                    {
                        level.Logs.Log($"Tried to speed the construction of a trap which is not in construction with game ID {BuildableGameID}.");
                    }
                    else
                    {
                        var speedUpCost = LogicUtils.CalculateSpeedUpCost(level.Assets, trap.ConstructionDuration);
                        level.Avatar.Gems -= speedUpCost;

                        trap.SpeedUpConstruction(Tick);
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
