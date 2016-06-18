using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents a part of a <see cref="Message"/>. This can be refereed as 'components'
    /// </summary>
    /// <remarks>
    /// This is to make <see cref="Message"/> implementations more modular.
    /// </remarks>
    public abstract class MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageComponent"/> class.
        /// </summary>
        public MessageComponent()
        {
            // Space
        }

        /// <summary>
        /// Reads the <see cref="MessageComponent"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="MessageComponent"/>.
        /// </param>
        /// <exception cref="NotImplementedException"><see cref="ReadMessageComponent(MessageReader)"/> is not implemented.</exception>
        public virtual void ReadMessageComponent(MessageReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the <see cref="Message"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Message"/>.
        /// </param>
        /// <exception cref="NotImplementedException"><see cref="WriteMessageComponent(MessageWriter)"/> is not implemented.</exception>
        public virtual void WriteMessageComponent(MessageWriter writer)
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
