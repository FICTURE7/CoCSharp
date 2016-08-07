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
            Unknown1 = 3;
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
        /// UserName
        /// </summary>
        public string Name;
        /// <summary>
        /// User Level
        /// </summary>
        public int Level;
        /// <summary>
        /// User League
        /// </summary>
        public int League;
        /// <summary>
        /// User Level
        /// </summary>
        public ClanMemberRole Role;
        /// <summary>
        /// UserName
        /// </summary>
        public string Message;
        private DateTime m_vMessageTime;
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
            writer.Write((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - (int)m_vMessageTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            writer.Write(Message);
        }
    }
}
