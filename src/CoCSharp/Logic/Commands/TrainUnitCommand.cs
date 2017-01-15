using System;
using CoCSharp.Network;
using CoCSharp.Logic.Components;
using CoCSharp.Csv;
using CoCSharp.Data.Slots;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server
    /// to tell it that units were trained.
    /// </summary>
    public class TrainUnitCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrainUnitCommand"/> class.
        /// </summary>
        public TrainUnitCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="TrainUnitCommand"/>.
        /// </summary>
        public override int Id => 508;

        /// <summary>
        /// Building that is going to train the units.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown2;

        /// <summary>
        /// Data ID of the unit.
        /// </summary>
        public int UnitDataID;
        /// <summary>
        /// Number of units to train.
        /// </summary>
        public int Count;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown3;

        /// <summary>
        /// Reads the <see cref="TrainUnitCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="TrainUnitCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();

            UnitDataID = reader.ReadInt32();
            Count = reader.ReadInt32();

            Unknown3 = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="TrainUnitCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="TrainUnitCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);

            writer.Write(UnitDataID);
            writer.Write(Count);

            writer.Write(Unknown3);

            writer.Write(Tick);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            //TODO: Implement TickTimer for training.

            var slot = level.Avatar.Units.GetSlot(UnitDataID);
            if (slot == null)
                level.Avatar.Units.Add(new UnitSlot(UnitDataID, Count));
            else
                slot.Amount += Count;
        }
    }
}
