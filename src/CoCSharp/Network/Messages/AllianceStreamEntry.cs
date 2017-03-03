using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents an alliance stream entry.
    /// </summary>
    public abstract class AllianceStreamEntry : StreamEntry
    {
        private DateTime _dateSent;
        private TimeSpan _sinceOccured;

        /// <summary>
        /// Gets the time at which it was sent.
        /// </summary>
        public DateTime DateSent => _dateSent;

        /// <summary>
        /// Entry ID.
        /// </summary>
        public long EntryId;

        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public byte Unknown2;

        /// <summary>
        /// User ID of the member that sent the entry.
        /// </summary>
        public long UserId;
        /// <summary>
        /// Home ID of the member that sent the entry.
        /// </summary>
        public long HomeId;
        /// <summary>
        /// name of the member that sent the entry.
        /// </summary>
        public string Name;
        /// <summary>
        /// Level of the member that sent the entry.
        /// </summary>
        public int ExpLevels;
        /// <summary>
        /// League of the member that sent the entry.
        /// </summary>
        public int League;
        /// <summary>
        /// Role of the member that sent the entry.
        /// </summary>
        public ClanMemberRole Role;

        /// <summary>
        /// Amount of time since the entry was sent.
        /// </summary>
        public TimeSpan SinceOccured
        {
            get
            {
                return _sinceOccured;
            }
            set
            {
                _sinceOccured = value;
                _dateSent = DateTime.UtcNow - value;
            }
        }

        /// <summary>
        /// Writes the <see cref="AllianceStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(EntryId);

            writer.Write(Unknown2);

            writer.Write(UserId);
            writer.Write(HomeId);
            writer.Write(Name);
            writer.Write(ExpLevels);
            writer.Write(League);
            writer.Write((int)Role);
            writer.Write((int)SinceOccured.TotalSeconds);
        }

        /// <summary>
        /// Reads the <see cref="AllianceStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            EntryId = reader.ReadInt64();

            Unknown2 = reader.ReadByte();

            UserId = reader.ReadInt64();
            HomeId = reader.ReadInt64();
            Name = reader.ReadString();
            ExpLevels = reader.ReadInt32();
            League = reader.ReadInt32();
            Role = (ClanMemberRole)reader.ReadInt32();
            SinceOccured = TimeSpan.FromSeconds(reader.ReadInt32());
        }

        public virtual void Update()
        {
            SinceOccured = DateTime.UtcNow - DateSent;
        }
    }
}
