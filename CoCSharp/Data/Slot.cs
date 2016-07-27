using CoCSharp.Network;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Base <see cref="Slot"/> class.
    /// </summary>
    public abstract class Slot
    {
        #region Constructors
        internal Slot()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        private int _id;
        /// <summary>
        /// Gets or sets the data ID of the <see cref="Slot"/>.
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (InvalidDataID(value))
                    throw new ArgumentOutOfRangeException("value", GetArgsOutOfRangeMessage("value"));

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

        /// <summary>Throws ArgumentNullException if reader is null.</summary>
        protected void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
        }

        /// <summary>Throws ArgumentNullException if writer is null.</summary>
        protected void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
        }

        internal virtual bool InvalidDataID(int dataId)
        {
            return false;
        }

        internal virtual int GetIndex(int dataId)
        {
            return dataId % InternalConstants.IDBase;
        }

        internal virtual string GetArgsOutOfRangeMessage(string paramName)
        {
            return null;
        }
        #endregion
    }
}
