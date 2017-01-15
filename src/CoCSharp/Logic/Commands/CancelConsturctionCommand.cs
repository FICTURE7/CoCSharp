using CoCSharp.Logic;
using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a construction was cancelled.
    /// </summary>
    public class CancelConsturctionCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelConsturctionCommand"/> class.
        /// </summary>
        public CancelConsturctionCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="CancelConsturctionCommand"/>.
        /// </summary>
        public override int Id { get { return 505; } }

        /// <summary>
        /// Game ID of the <see cref="VillageObject"/> whose
        /// construction was cancelled.
        /// </summary>
        public int VillageObjectID;

        /// <summary>
        /// Reads the <see cref="CancelConsturctionCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="CancelConsturctionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <summary/>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            VillageObjectID = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="CancelConsturctionCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="CancelConsturctionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(VillageObjectID);

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="CancelConsturctionCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="CancelConsturctionCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            var vilobj = village.VillageObjects[VillageObjectID];
            if (vilobj == null)
            {
                level.Logs.Log($"Could not find village object with game ID {VillageObjectID}.");
            }
            else
            {
                if (vilobj is Building)
                {
                    var building = (Building)vilobj;
                    if (!building.IsConstructing)
                    {
                        level.Logs.Log($"Tried to cancel the construction of a building which is not in construction with game ID {VillageObjectID}.");
                    }
                    else
                    {
                        building.CancelConstruction(Tick);
                    }
                }
                else if (vilobj is Trap)
                {
                    var trap = (Trap)vilobj;
                    if (!trap.IsConstructing)
                    {
                        level.Logs.Log($"Tried to cancel the construction of a trap which is not in construction with game ID {VillageObjectID}.");
                    }
                    else
                    {
                        trap.CancelConstruction(Tick);
                    }
                }
                else if (vilobj is Obstacle)
                {
                    var obstacle = (Obstacle)vilobj;
                    if (!obstacle.IsClearing)
                    {
                        level.Logs.Log($"Tried to cancel the clearing of a obstacle which is not in construction with game ID {VillageObjectID}.");
                    }
                    else
                    {
                        obstacle.CancelClearing(Tick);
                    }
                }
                else
                {
                    level.Logs.Log($"Unexpected VillageObject type: {vilobj.GetType().Name} was asked to be canceled.");
                }
            }
        }
    }
}
