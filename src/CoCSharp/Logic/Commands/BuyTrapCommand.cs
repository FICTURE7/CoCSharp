using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a trap was bought.
    /// </summary>
    public class BuyTrapCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyTrapCommand"/> class.
        /// </summary>
        public BuyTrapCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyTrapCommand"/>.
        /// </summary>
        public override int Id { get { return 510; } }

        /// <summary>
        /// X coordinates of the <see cref="Trap"/> where it was placed.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the <see cref="Trap"/> where it was placed.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data ID of the <see cref="Trap"/> that was bought.
        /// </summary>
        public int TrapDataID;

        /// <summary>
        /// Reads the <see cref="BuyTrapCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyTrapCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            TrapDataID = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyTrapCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyTrapCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(TrapDataID);

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="BuyTrapCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="BuyTrapCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var dataRef = new CsvDataRowRef<TrapData>(TrapDataID);
            var assets = level.Assets;
            var tableCollection = assets.DataTables;
            var row = dataRef.Get(tableCollection);
            if (row == null)
            {
                level.Logs.Log($"Unable to find CsvDataRow<TrapData> for data ID {TrapDataID}.");
            }
            else
            {
                // Use the first level.
                var data = row[0];
                if (data == null)
                {
                    level.Logs.Log($"Unable to find TrapData of level 0 for data ID {TrapDataID}.");
                }
                else
                {
                    level.Avatar.UseResource(data.BuildResource, data.BuildCost);

                    var trap = new Trap(level.Village, data);
                    trap.X = X;
                    trap.Y = Y;
                    trap.BeginConstruction(Tick);
                }
            }
        }
    }
}