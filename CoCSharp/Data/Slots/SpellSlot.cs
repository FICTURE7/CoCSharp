using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans spell slot.
    /// </summary>
    public class SpellSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellSlot"/> class.
        /// </summary>
        public SpellSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellSlot"/> class with
        /// the specified spell ID and amount.
        /// </summary>
        /// <param name="id">ID of the spell.</param>
        /// <param name="amount">Amount of the spell.</param>
        public SpellSlot(int id, int amount)
        {
            ID = id;
            Amount = amount;
        }

        /// <summary>
        /// Gets or sets the spell ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the amount of spell.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Reads the <see cref="SpellSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="SpellSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt32();
            Amount = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="SpellSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="SpellSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(Amount);
        }
    }
}
