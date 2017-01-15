using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans hero upgrade slot.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, Level = {Level}")]
    public class HeroUpgradeSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeroUpgradeSlot"/> class.
        /// </summary>
        public HeroUpgradeSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeroUpgradeSlot"/> class with
        /// the specified hero upgrade ID and amount.
        /// </summary>
        /// <param name="id">ID of the hero upgrade.</param>
        /// <param name="level">Level of the hero upgrade.</param>
        public HeroUpgradeSlot(int id, int level)
        {
            Id = id;
            Level = level;
        }

        /// <summary>
        /// Gets or sets the level of hero upgrade.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Reads the <see cref="HeroUpgradeSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="HeroUpgradeSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadInt32();
            Level = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="HeroUpgradeSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="HeroUpgradeSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id);
            writer.Write(Level);
        }
    }
}
