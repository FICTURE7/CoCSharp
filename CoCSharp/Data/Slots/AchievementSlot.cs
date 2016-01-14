using CoCSharp.Networking;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans achievment slot.
    /// </summary>
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
            ID = id;
        }

        /// <summary>
        /// Gets or sets the achievement ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Reads the <see cref="AchievementSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AchievementSlot"/>.
        /// </param>
        public override void ReadSlot(MessageReader reader)
        {
            ID = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AchievementSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AchievementSlot"/>.
        /// </param>
        public override void WriteSlot(MessageWriter writer)
        {
            writer.Write(ID);
        }
    }
}
