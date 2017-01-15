﻿using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents an unknown slot.
    /// </summary>
    [DebuggerDisplay("ID = {Id}, Value = {Value}")]
    public class UnknownSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownSlot"/> class.
        /// </summary>
        public UnknownSlot()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the value of the unknown slot.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Reads the <see cref="UnknownSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UnknownSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadInt32();
            Value = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="UnknownSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UnknownSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id);
            writer.Write(Value);
        }
    }
}
