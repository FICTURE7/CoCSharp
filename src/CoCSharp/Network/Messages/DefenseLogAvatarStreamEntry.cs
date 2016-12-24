using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Stream entry sent to give the client information about a defense log
    /// entry.
    /// </summary>
    public class DefenseLogAvatarStreamEntry : AvatarStreamEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefenseLogAvatarStreamEntry"/> class.
        /// </summary>
        public DefenseLogAvatarStreamEntry()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="DefenseLogAvatarStreamEntry"/>.
        /// </summary>
        public override int Id { get { return 2; } }

        /// <summary>
        /// Unknown long 1.
        /// </summary>
        public long Unknown1;
        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public byte Unknown2;

        /// <summary>
        /// User ID of the avatar associated with this
        /// entry.
        /// </summary>
        public long UserId;
        /// <summary>
        /// Username of the avatar associated with this
        /// entry.
        /// </summary>
        public string Username;

        /// <summary>
        /// Unknown integer 5.
        /// </summary>
        public int Unknown5;
        /// <summary>
        /// Unknown integer 6.
        /// </summary>
        public int Unknown6;

        /// <summary>
        /// Age of the log entry.
        /// </summary>
        public TimeSpan LogAge;

        /// <summary>
        /// Unknown integer 8.
        /// </summary>
        public byte Unknown8;

        /// <summary>
        /// JSON about details of the log.
        /// </summary>
        public string Details;

        /// <summary>
        /// Unknown byte 9.
        /// </summary>
        public byte Unknown9;
        /// <summary>
        /// Unknown integer 10.
        /// </summary>
        public int Unknown10;
        /// <summary>
        /// Unknown integer 11.
        /// </summary>
        public int Unknown11;
        /// <summary>
        /// Unknown integer 12.
        /// </summary>
        public int Unknown12;
        /// <summary>
        /// Unknown extension.
        /// </summary>
        public Extension UnknownExtension;

        /// <summary>
        /// Reads the <see cref="DefenseLogAvatarStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="DefenseLogAvatarStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt64();
            Unknown2 = reader.ReadByte();

            UserId = reader.ReadInt64();
            Username = reader.ReadString();

            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();

            LogAge = TimeSpan.FromSeconds(reader.ReadInt32());

            Unknown8 = reader.ReadByte();

            Details = reader.ReadString();

            Unknown9 = reader.ReadByte();
            Unknown10 = reader.ReadInt32();
            Unknown11 = reader.ReadInt32();
            Unknown12 = reader.ReadInt32();
            if (reader.ReadBoolean())
            {
                UnknownExtension = new Extension();
                UnknownExtension.Unknown1 = reader.ReadInt32();
                UnknownExtension.Unknown2 = reader.ReadInt64();
            }
        }

        /// <summary>
        /// Writes the <see cref="DefenseLogAvatarStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="DefenseLogAvatarStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);

            writer.Write(UserId);
            writer.Write(Username);

            writer.Write(Unknown5);
            writer.Write(Unknown6);

            writer.Write((int)LogAge.TotalSeconds);

            writer.Write(Unknown8);

            writer.Write(Details);

            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);
            writer.Write(Unknown12);
            if (UnknownExtension != null)
            {
                writer.Write(UnknownExtension.Unknown1);
                writer.Write(UnknownExtension.Unknown2);
            }
        }

        /// <summary>
        /// Extension of <see cref="DefenseLogAvatarStreamEntry"/>.
        /// </summary>
        public class Extension
        {
            /// <summary>
            /// Unknown integer 1.
            /// </summary>
            public int Unknown1;
            /// <summary>
            /// Unknown long 2.
            /// </summary>
            public long Unknown2;
        }
    }
}
