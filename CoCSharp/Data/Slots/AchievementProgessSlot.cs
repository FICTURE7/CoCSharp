using CoCSharp.Network;
using System;

namespace CoCSharp.Data.Slots
{
    /// <summary>
    /// Represents a Clash of Clans achievement progress slot.
    /// </summary>
    public class AchievementProgessSlot : Slot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AchievementProgessSlot"/> class.
        /// </summary>
        public AchievementProgessSlot()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AchievementProgessSlot"/> class with
        /// the specified achievement ID and progress.
        /// </summary>
        /// <param name="id">ID of the achievement.</param>
        /// <param name="progress">Progress of the achievement.</param>
        public AchievementProgessSlot(int id, int progress)
        {
            ID = id;
            Progress = progress;
        }

        /// <summary>
        /// Gets or sets the achievement ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the progress of achievement.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Reads the <see cref="AchievementProgessSlot"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AchievementProgessSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadSlot(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt32();
            Progress = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AchievementProgessSlot"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AchievementProgessSlot"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteSlot(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(Progress);
        }
    }
}
