using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans hero state slot.
    /// </summary>
    public class HeroStateSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeroStateSlot"/> class.
        /// </summary>
        public HeroStateSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeroStateSlot"/> class with
        /// the specified hero ID and state.
        /// </summary>
        /// <param name="id">ID of the hero.</param>
        /// <param name="state">State of the hero.</param>
        public HeroStateSlot(int id, int state)
        {
            ID = id;
            State = state;
        }

        /// <summary>
        /// Gets or sets the hero ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the state of hero.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Reads the <see cref="HeroStateSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="HeroStateSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt32();
            State = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="HeroStateSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="HeroStateSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(State);
        }

    }
}
