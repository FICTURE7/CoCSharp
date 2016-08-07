using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.AlllianceStream
{
    /// <summary>
    /// Stream entry sent to give the client to comunicate in clan
    /// entry.
    /// </summary>
    public class ChatStreamEntry : AllianceStreamEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatStreamEntry"/> class.
        /// </summary>
        public ChatStreamEntry()
        {
            //space
        }

        /// <summary>
        /// Message Specific ID.
        /// </summary>
        public long MessageID;
        /// <summary>
        /// Unknown Byte1.
        /// </summary>
        public byte Unknown1;
        /// <summary>
        /// User ID
        /// </summary>
        public long UserID;
        /// <summary>
        /// user Home ID
        /// </summary>
        public long HomeID;
        /// <summary>
        /// User Current Name
        /// </summary>
        public string Name;
        /// <summary>
        /// Level to user is currently at
        /// </summary>
        public int Level;
        /// <summary>
        /// League that user is currently in
        /// </summary>
        public int League;
        /// <summary>
        /// User Level
        /// </summary>
        public ClanMemberRole Role;
        /// <summary>
        /// Message that going to be sent
        /// </summary>
        public string Message;
        /// <summary>
        /// Get the time of the message being sended
        /// </summary>
        private int MessageTime;
        /// <summary>
        /// Get the ID of the <see cref="ChatStreamEntry"/>
        /// </summary>
        public override int ID
        {
            get
            {
                return 2;
            }
        }
        // <summary>
        /// Reads the <see cref="ChatStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="TroopRequestStreamEntry"/>.
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

            MessageTime = reader.ReadInt32();
            Message = reader.ReadString();

        }

        /// <summary>
        /// Writes the <see cref="ChatStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChatStreamEntry"/>.
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
            writer.Write(MessageTime); //(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - (int)MessageTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) But ficture want other thing
            writer.Write(Message);
        }
    }
}
