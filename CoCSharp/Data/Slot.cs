using CoCSharp.Networking;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a Clash of Clans slot.
    /// </summary>
    public abstract class Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        public Slot()
        {
            // Space
        }

        /// <summary>
        /// Reads the <see cref="Slot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="Slot"/>.
        /// </param>
        public abstract void ReadSlot(MessageReader reader);

        /// <summary>
        /// Writes the <see cref="Slot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Slot"/>.
        /// </param>
        public abstract void WriteSlot(MessageWriter writer);

        /// <summary>
        /// Writes the <see cref="Slot"/> array to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Slot"/>.
        /// </param>
        /// <param name="slots"><see cref="Slot"/> array to write.</param>
        public static void WriteSlotArray(MessageWriter writer, Slot[] slots)
        {
            if (slots == null)
            {
                writer.Write(0);
                return;
            }

            var count = slots.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
                slots[i].WriteSlot(writer);
        }

        /// <summary>
        /// Reads the specified <see cref="Slot"/> type array from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Slot"/> type.</typeparam>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="Slot"/> array.
        /// </param>
        /// <returns><see cref="Slot"/> array read.</returns>
        public static T[] ReadSlotArray<T>(MessageReader reader) where T : Slot
        {
            var type = typeof(T);
            var count = reader.ReadInt32();
            var array = new T[count];

            for (int i = 0; i < count; i++)
            {
                var instance = (T)Activator.CreateInstance(type);
                instance.ReadSlot(reader);
                array[i] = instance;
            }
            return array;
        }
    }
}
