using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a decoration was sold.
    /// </summary>
    public class SellDecorationCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SellDecorationCommand"/> class.
        /// </summary>
        public SellDecorationCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="SellDecorationCommand"/>.
        /// </summary>
        public override int ID { get { return 503; } }

        /// <summary>
        /// Game ID of the <see cref="Decoration"/> that was sold.
        /// </summary>
        public int DecorationGameID;

        /// <summary>
        /// Reads the <see cref="SellDecorationCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="SellDecorationCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            DecorationGameID = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="SellDecorationCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="SellDecorationCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(DecorationGameID);

            writer.Write(Tick);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            var vilobj = village.VillageObjects[DecorationGameID];
            if (vilobj == null)
            {
                Debug.WriteLine($"Could not find village object with ID: {DecorationGameID}");
                return;
            }

            var deco = (Decoration)vilobj;
            var data = deco.Data;
            var buildCost = data.BuildCost;
            var buildResource = data.BuildResource;

            // 10% of build cost.
            var refund = (int)Math.Round(0.1d * data.BuildCost);

            level.Avatar.ConsumeResource(buildResource, -refund);
            village.VillageObjects.Remove(DecorationGameID);
        }
    }
}
