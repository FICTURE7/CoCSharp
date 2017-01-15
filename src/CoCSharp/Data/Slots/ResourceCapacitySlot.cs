﻿using CoCSharp.Data.Models;
using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans resource capacity slot.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, Capacity = {Capacity}")]
    public class ResourceCapacitySlot : Slot<ResourceData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCapacitySlot"/> class.
        /// </summary>
        public ResourceCapacitySlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCapacitySlot"/> class with
        /// the specified resource ID and capacity.
        /// </summary>
        /// <param name="id">ID of the resource.</param>
        /// <param name="capacity">Capacity of the resource.</param>
        public ResourceCapacitySlot(int id, int capacity)
        {
            Id = id;
            Capacity = capacity;
        }

        /// <summary>
        /// Gets or sets the resource capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Reads the <see cref="ResourceCapacitySlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ResourceCapacitySlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader) 
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadInt32();
            Capacity = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="ResourceCapacitySlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ResourceCapacitySlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id);
            writer.Write(Capacity);
        }
    }
}
