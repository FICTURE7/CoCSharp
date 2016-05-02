using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client after
    /// a <see cref="ChatMessageClientMessage"/>.
    /// </summary>
    public class ChatMessageServerMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageServerMessage"/> class.
        /// </summary>
        public ChatMessageServerMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="ChatMessageServerMessage"/>.
        /// </summary>
        public override ushort ID { get { return 24715; } }

        /// <summary>
        /// Message that will be sent to the lobby.
        /// </summary>
        public string Message;
        /// <summary>
        /// Name of the sender.
        /// </summary>
        public string Name;
        /// <summary>
        /// Level of the sender.
        /// </summary>
        public int Level;
        /// <summary>
        /// League of the sender.
        /// </summary>
        public int League;
        /// <summary>
        /// User ID of the sender.
        /// </summary>
        public long UserID;
        /// <summary>
        /// Current user ID of the sender.
        /// </summary>
        public long CurrentUserID;
        /// <summary>
        /// Clan of the sender.
        /// </summary>
        public Clan Clan;

        /// <summary>
        /// Reads the <see cref="ChatMessageServerMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChatMessageServerMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Message = reader.ReadString();
            Name = reader.ReadString();
            Level = reader.ReadInt32();
            League = reader.ReadInt32();
            UserID = reader.ReadInt64();
            CurrentUserID = reader.ReadInt64();

            Clan = new Clan();
            if (reader.ReadBoolean())
            {
                Clan.ID = reader.ReadInt64();
                Clan.Name = reader.ReadString();
                Clan.Badge = reader.ReadInt32();
            }
        }

        /// <summary>
        /// Writes the <see cref="ChatMessageServerMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChatMessageServerMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Message);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write(League);
            writer.Write(UserID);
            writer.Write(CurrentUserID);

            if (Clan != null)
            {
                writer.Write(Clan.ID);
                writer.Write(Clan.Name);
                writer.Write(Clan.Level);
            }
        }
    }
}
