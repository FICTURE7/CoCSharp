using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans alliance unit slot.
    /// </summary>
    public class AllianceUnitSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceUnitSlot"/> class.
        /// </summary>
        public AllianceUnitSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceUnitSlot"/> class with
        /// the specified unit ID and amount.
        /// </summary>
        /// <param name="id">ID of the unit.</param>
        /// <param name="amount">Amount of the unit.</param>
        /// <param name="level">Level of the unit.</param>
        public AllianceUnitSlot(int id, int amount, int level)
        {
            ID = id;
            Amount = amount;
        }

        /// <summary>
        /// Gets or sets the unit ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the amount of unit.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the level of unit.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Reads the <see cref="AllianceUnitSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceUnitSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);
            
            ID = reader.ReadInt32();
            Amount = reader.ReadInt32();
            Level = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AllianceUnitSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceUnitSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(Amount);
            writer.Write(Level);
        }
    }
}
