using CoCSharp.Network;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Extension methods for <see cref="MessageReader"/> and <see cref="MessageWriter"/>.
    /// </summary>
    public static class MessageReaderWriterExt
    {
        /// <summary>
        /// Writes a <see cref="SlotCollection{TSlot}"/> to the current stream.
        /// </summary>
        /// <typeparam name="TSlot">Type of <see cref="Slot"/> to read.</typeparam>
        /// <param name="writer"><see cref="MessageWriter"/> to use.</param>
        /// <param name="slots">The <see cref="SlotCollection{TSlot}"/> to write.</param>
        public static void WriteSlotCollection<TSlot>(this MessageWriter writer, SlotCollection<TSlot> slots) where TSlot : Slot, new()
        {
            writer.CheckDispose();

            if (slots == null)
                writer.Write(0);
            else
            {
                var count = slots.Count;

                writer.Write(count);
                for (int i = 0; i < count; i++)
                    slots[i].WriteSlot(writer);
            }
        }

        /// <summary>
        /// Reads a <see cref="SlotCollection{TSlot}"/> from the current stream.
        /// </summary>
        /// <param name="reader"><see cref="MessageReader"/> to use.</param>
        /// <typeparam name="TSlot">Type of <see cref="Slot"/> to read.</typeparam>
        /// <returns>A <see cref="SlotCollection{TSlot}"/> read from the current stream.</returns>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageReader"/> is closed.</exception>
        /// <exception cref="InvalidMessageException">Slot collection length is invalid.</exception>
        public static SlotCollection<TSlot> ReadSlotCollection<TSlot>(this MessageReader reader) where TSlot : Slot, new()
        {
            reader.CheckDispose();

            var count = reader.ReadInt32();
            if (count < 0)
                throw new InvalidMessageException("Invalid slot array size '" + count + "'.");

            var collection = new SlotCollection<TSlot>();
            for (int i = 0; i < count; i++)
            {
                var instance = new TSlot();
                instance.ReadSlot(reader);

                collection.Add(instance);
            }
            return collection;
        }
    }
}
