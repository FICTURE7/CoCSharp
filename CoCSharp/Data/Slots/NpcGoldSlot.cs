using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans npc gold slot.
    /// </summary>
    public class NpcGoldSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcGoldSlot"/> class.
        /// </summary>
        public NpcGoldSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcStarSlot"/> class with
        /// the specified NPC ID and looted gold amount.
        /// </summary>
        /// <param name="id">ID of the NPC.</param>
        /// <param name="gold">Looted gold amount.</param>
        public NpcGoldSlot(int id, int gold)
        {
            ID = id;
            Gold = gold;
        }

        /// <summary>
        /// Gets or sets the NPC ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the looted gold amount.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// Reads the <see cref="NpcGoldSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="NpcGoldSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt32();
            Gold = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="NpcGoldSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="NpcGoldSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(Gold);
        }
    }
}
