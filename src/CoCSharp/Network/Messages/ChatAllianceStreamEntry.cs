using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Stream entry indicating chat messages sent in
    /// the alliance chat.
    /// </summary>
    public class ChatAllianceStreamEntry : AllianceStreamEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatAllianceStreamEntry"/> class.
        /// </summary>
        public ChatAllianceStreamEntry()
        {
            //space
        }

        /// <summary>
        /// Message ID.
        /// </summary>
        public long MessageID;
        
        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;

        /// <summary>
        /// User ID of the member that sent the message.
        /// </summary>
        public long UserID;
        /// <summary>
        /// Home ID of the member that sent the message.
        /// </summary>
        public long HomeID;
        /// <summary>
        /// name of the member that sent the message.
        /// </summary>
        public string Name;
        /// <summary>
        /// Level of the member that sent the message.
        /// </summary>
        public int Level;
        /// <summary>
        /// League of the member that sent the message.
        /// </summary>
        public int League;
        /// <summary>
        /// Role of the member that sent the message.
        /// </summary>
        public ClanMemberRole Role;
        /// <summary>
        /// Message that was sent.
        /// </summary>
        public string Message;
        /// <summary>
        /// Time at which the message was sent.
        /// </summary>
        private DateTime MessageTime;

        /// <summary>
        /// Get the ID of the <see cref="ChatAllianceStreamEntry"/>
        /// </summary>
        public override int ID
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Reads the <see cref="ChatAllianceStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChatAllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            MessageID = reader.ReadInt32();

            Unknown1 = reader.ReadByte();

            UserID = reader.ReadInt64();
            HomeID = reader.ReadInt64();
            Name = reader.ReadString();
            Level = reader.ReadInt32();
            League = reader.ReadInt32();
            Role = (ClanMemberRole)reader.ReadInt32();
            MessageTime = TimeUtils.FromUnixTimestamp(reader.ReadInt32());
            Message = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="ChatAllianceStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChatAllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(MessageID);

            writer.Write(Unknown1);

            writer.Write(UserID);
            writer.Write(HomeID);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write(League);
            writer.Write((int)Role);
            writer.Write(TimeUtils.ToUnixTimestamp(MessageTime));
            writer.Write(Message);
        }
    }
}
