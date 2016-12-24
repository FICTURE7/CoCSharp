using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents a stream entry.
    /// </summary>
    public abstract class StreamEntry
    {
        /// <summary>
        /// Gets the ID of the <see cref="StreamEntry"/>.
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// Reads the <see cref="StreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="StreamEntry"/>.
        /// </param>
        public virtual void ReadStreamEntry(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="StreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="StreamEntry"/>.
        /// </param>
        public virtual void WriteStreamEntry(MessageWriter writer)
        {
            // Space
        }

        internal void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
        }

        internal void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
        }
    }
}
