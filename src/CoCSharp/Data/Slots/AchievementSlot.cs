using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans achievement slot.
    /// </summary>
    [DebuggerDisplay("ID = {ID}")]
    public class AchievementSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AchievementSlot"/> class.
        /// </summary>
        public AchievementSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AchievementSlot"/> class with
        /// the specified achievement ID.
        /// </summary>
        /// <param name="id">ID of the achievement.</param>
        public AchievementSlot(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Reads the <see cref="AchievementSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AchievementSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AchievementSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AchievementSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id);
        }
    }
}
