using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client when a alliance search is done.
    /// </summary>
    public class AllianceSearchResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceSearchResponseMessage"/> class.
        /// </summary>
        public AllianceSearchResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceSearchResponseMessage"/>.
        /// </summary>
        public override ushort Id => 24310;

        /// <summary>
        /// Search string.
        /// </summary>
        public string TextSearch;
        /// <summary>
        /// Clans that matches the search.
        /// </summary>
        public ClanCompleteMessageComponent[] Clans;

        /// <summary>
        /// Reads the <see cref="AllianceSearchResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceSearchResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            TextSearch = reader.ReadString();

            var count = reader.ReadInt32();
            Clans = new ClanCompleteMessageComponent[count];
            for (int i = 0; i < Clans.Length; i++)
            {
                var clan = new ClanCompleteMessageComponent();
                clan.ReadMessageComponent(reader);
                Clans[i] = clan;
            }
        }

        /// <summary>
        /// Writes the <see cref="AllianceSearchResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceSearchResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(TextSearch);

            if (Clans == null)
            {
                writer.Write(0);
                return;
            }

            writer.Write(Clans.Length);
            for (int i = 0; i < Clans.Length; i++)
            {
                var clan = Clans[i];
                if (clan == null)
                    throw new InvalidOperationException("Encountered null CompleteClanMessageComponent.");
                clan.WriteMessageComponent(writer);
            }
        }
    }
}
