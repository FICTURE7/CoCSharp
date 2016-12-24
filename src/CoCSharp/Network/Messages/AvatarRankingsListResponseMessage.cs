using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Description of AvatarRankingsListResponseMessage.
    /// </summary>
    public class AvatarRankingsListResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingsListResponseMessage"/> class.
        /// </summary>
        public AvatarRankingsListResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AvatarRankingsListResponseMessage"/>.
        /// </summary>
        public override ushort Id => 24403;

        /// <summary>
        /// List of avatar rankings.
        /// </summary>
        public AvatarRankingEntryMessageComponent[] Rankings;
        /// <summary>
        /// List of top 3 avatar rankings.
        /// </summary>
        public AvatarRankingEntryMessageComponent[] TopRankings;

        /// <summary>
        /// Time until the season ends.
        /// </summary>
        public TimeSpan RemaingTimeUntilSeasonEnds;
        /// <summary>
        /// Current season's year.
        /// </summary>
        public int SeasonYear;
        /// <summary>
        /// Current season's month.
        /// </summary>
        public int SeasonMonth;
        /// <summary>
        /// Previous season's year.
        /// </summary>
        public int PreviousSeasonYear;
        /// <summary>
        /// Previous season's month.
        /// </summary>
        public int PreviousSeasonMonth;

        /// <summary>
        /// Reads the <see cref="AvatarRankingsListResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarRankingsListResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            var count = reader.ReadInt32();
            Rankings = new AvatarRankingEntryMessageComponent[count];

            for (int i = 0; i < count; i++)
            {
                var avatar = new AvatarRankingEntryMessageComponent();
                avatar.ReadMessageComponent(reader);

                Rankings[i] = avatar;
            }

            var count2 = reader.ReadInt32();
            TopRankings = new AvatarRankingEntryMessageComponent[count2];
            for (int i = 0; i < count2; i++)
            {
                var avatar = new AvatarRankingEntryMessageComponent();
                avatar.ReadMessageComponent(reader);

                TopRankings[i] = avatar;
            }

            RemaingTimeUntilSeasonEnds = TimeSpan.FromSeconds(reader.ReadInt32());
            SeasonYear = reader.ReadInt32();
            SeasonMonth = reader.ReadInt32();
            PreviousSeasonYear = reader.ReadInt32();
            PreviousSeasonMonth = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AvatarRankingsListResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarRankingsListResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (Rankings == null || Rankings.Length == 0)
            {
                writer.Write(0);
            }
            else
            {
                writer.Write(Rankings.Length);
                for (int i = 0; i < Rankings.Length; i++)
                    Rankings[i].WriteMessageComponent(writer);
            }

            if (TopRankings == null || TopRankings.Length == 0)
            {
                writer.Write(0);
            }
            else
            {
                writer.Write(TopRankings.Length);
                for (int i = 0; i < TopRankings.Length; i++)
                    TopRankings[i].WriteMessageComponent(writer);
            }

            writer.Write((int)RemaingTimeUntilSeasonEnds.TotalSeconds);
            writer.Write(SeasonYear);
            writer.Write(SeasonMonth);
            writer.Write(PreviousSeasonYear);
            writer.Write(PreviousSeasonMonth);
        }
    }
}
