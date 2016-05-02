using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans tutorial(misison) progress slot.
    /// </summary>
    public class TutorialProgressSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialProgressSlot"/> class.
        /// </summary>
        public TutorialProgressSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialProgressSlot"/> class with
        /// the specified mission ID.
        /// </summary>
        /// <param name="id">ID of the mission.</param>
        public TutorialProgressSlot(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Gets or sets the mission ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Reads the <see cref="TutorialProgressSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="TutorialProgressSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="TutorialProgressSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="TutorialProgressSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
        }
    }
}
