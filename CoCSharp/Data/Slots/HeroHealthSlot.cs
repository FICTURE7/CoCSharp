using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans hero health slot.
    /// </summary>
    public class HeroHealthSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeroHealthSlot"/> class.
        /// </summary>
        public HeroHealthSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeroHealthSlot"/> class with
        /// the specified hero ID and health.
        /// </summary>
        /// <param name="id">ID of the hero.</param>
        /// <param name="health">Health of the hero.</param>
        public HeroHealthSlot(int id, int health)
        {
            ID = id;
            Health = health;
        }

        /// <summary>
        /// Gets or sets the hero ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the health of hero.
        /// </summary>
        public int Health { get; set; }

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

            ID = reader.ReadInt32();
            Health = reader.ReadInt32();
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

            writer.Write(ID);
            writer.Write(Health);
        }
    }
}
