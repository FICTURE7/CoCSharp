using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a decoration was bought.
    /// </summary>
    public class BuyDecorationCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyDecorationCommand"/> class.
        /// </summary>
        public BuyDecorationCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyDecorationCommand"/>.
        /// </summary>
        public override int Id { get { return 512; } }

        /// <summary>
        /// X coordinates of the <see cref="Decoration"/> where it was placed.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the <see cref="Decoration"/> where it was placed.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data ID of the <see cref="Decoration"/> that was bought.
        /// </summary>
        public int DecorationDataID;

        /// <summary>
        /// Reads the <see cref="BuyDecorationCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyDecorationCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            DecorationDataID = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyDecorationCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyDecorationCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(DecorationDataID);

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="BuyDecorationCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="BuyDecorationCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var dataRef = new CsvDataRowRef<DecorationData>(DecorationDataID);
            var assets = level.Assets;
            var tableCollection = assets.DataTables;
            var row = dataRef.Get(tableCollection);
            if (row == null)
            {
                level.Logs.Log($"Unable to find CsvDataRow<DecorationData> for data ID {DecorationDataID}.");
            }
            else
            {
                // Use the first level.
                var data = row[0];
                if (data == null)
                {
                    level.Logs.Log($"Unable to find DecorationData of level 0 for data ID {DecorationDataID}.");
                }
                else
                {
                    level.Avatar.UseResource(data.BuildResource, data.BuildCost);

                    var deco = new Decoration(level.Village, data);
                    deco.X = X;
                    deco.Y = Y;
                }
            }
        }
    }
}
