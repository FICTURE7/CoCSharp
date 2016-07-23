using CoCSharp.Network;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Base <see cref="Slot"/> class.
    /// </summary>
    public abstract class Slot
    {
        internal Slot()
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

        // Throws ArgumentNullException when reader is null.
        internal void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
        }

        // Throws ArgumentNullException when writer is null.
        internal void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
        }
    }
}
