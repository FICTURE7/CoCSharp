using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans unit upgrade slot.
    /// </summary>
    public class UnitUpgradeSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitUpgradeSlot"/> class.
        /// </summary>
        public UnitUpgradeSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitUpgradeSlot"/> class with
        /// the specified unit upgrade ID and amount.
        /// </summary>
        /// <param name="id">ID of the unit upgrade.</param>
        /// <param name="level">Level of the unit upgrade.</param>
        public UnitUpgradeSlot(int id, int level)
        {
            ID = id;
            Level = level;
        }

        /// <summary>
        /// Gets or sets the unit upgrade ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the level of unit upgrade.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Reads the <see cref="UnitUpgradeSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UnitUpgradeSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);
            
            ID = reader.ReadInt32();
            Level = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="UnitUpgradeSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UnitUpgradeSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(Level);
        }
    }
}
