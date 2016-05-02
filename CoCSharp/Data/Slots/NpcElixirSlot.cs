using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans npc elixir slot.
    /// </summary>
    public class NpcElixirSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcElixirSlot"/> class.
        /// </summary>
        public NpcElixirSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcElixirSlot"/> class with
        /// the specified NPC ID and looted elixir amount.
        /// </summary>
        /// <param name="id">ID of the NPC.</param>
        /// <param name="elixir">Looted elixir amount.</param>
        public NpcElixirSlot(int id, int elixir)
        {
            ID = id;
            Elixir = elixir;
        }

        /// <summary>
        /// Gets or sets the NPC ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the looted elixir amount.
        /// </summary>
        public int Elixir { get; set; }

        /// <summary>
        /// Reads the <see cref="NpcElixirSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="NpcElixirSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt32();
            Elixir = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="NpcElixirSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="NpcElixirSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
            writer.Write(ID);
            writer.Write(Elixir);
        }
    }
}
