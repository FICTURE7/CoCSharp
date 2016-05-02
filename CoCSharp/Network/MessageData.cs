using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents a part of a <see cref="Message"/>.
    /// </summary>
    /// <remarks>
    /// This is to make <see cref="Message"/> implementation
    /// more modular.
    /// </remarks>
    public abstract class MessageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageData"/> class.
        /// </summary>
        public MessageData()
        {
            // Space
        }

        /// <summary>
        /// Reads the <see cref="MessageData"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="MessageData"/>.
        /// </param>
        /// <exception cref="NotImplementedException"><see cref="ReadMessageData(MessageReader)"/> is not implemented.</exception>
        public virtual void ReadMessageData(MessageReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the <see cref="Message"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Message"/>.
        /// </param>
        /// <exception cref="NotImplementedException"><see cref="WriteMessageData(MessageWriter)"/> is not implemented.</exception>
        public virtual void WriteMessageData(MessageWriter writer)
        {
            throw new NotImplementedException();
        }

        // Throws a ArgumentNullException if reader is null.
        internal void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
        }

        // Throws a ArgumentNullException if writer is null.
        internal void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
        }
    }
}
