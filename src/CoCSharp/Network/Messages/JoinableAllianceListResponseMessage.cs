using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client after the <see cref="JoinableAllianceListRequestMessage"/>
    /// to give the list of alliances the client is able to join.
    /// </summary>
    public class JoinableAllianceListResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListResponseMessage"/> class.
        /// </summary>
        public JoinableAllianceListResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="JoinableAllianceListResponseMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24304; } }

        /// <summary>
        /// Clans available to join.
        /// </summary>
        public ClanCompleteMessageComponent[] Clans;

        /// <summary>
        /// Reads the <see cref="JoinableAllianceListResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="JoinableAllianceListResponseMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
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
        /// Writes the <see cref="JoinableAllianceListResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="JoinableAllianceListResponseMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
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
