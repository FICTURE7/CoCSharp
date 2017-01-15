using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Data
{
    /// <summary>
    /// Base <see cref="Slot"/> class.
    /// </summary>
    [DebuggerDisplay("Id = {Id}")]
    public abstract class Slot
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        protected Slot()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        private int _id;

        /// <summary>
        /// Gets or sets the data ID of the <see cref="Slot"/>.
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                // Check if the specified Data ID is valid for this slot.
                if (InvalidDataId(value))
                    throw new ArgumentOutOfRangeException(nameof(value), GetArgsOutOfRangeMessage(nameof(value)));

                _id = value;
            }
        }
        #endregion

        #region Methods
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

        internal virtual bool InvalidDataId(int dataId)
        {
            return false;
        }

        internal virtual string GetArgsOutOfRangeMessage(string paramName)
        {
            return null;
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
        #endregion
    }
}
