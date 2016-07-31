using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the server to the client
    /// to tell it that it joined an alliance.
    /// </summary>
    public class JoinAllianceCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceCommand"/> class.
        /// </summary>
        public JoinAllianceCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="JoinAllianceCommand"/>.
        /// </summary>
        public override int ID { get { return 1; } }

        /// <summary>
        /// ID of the alliance the client joined.
        /// </summary>
        public long ClanID;
        /// <summary>
        /// Name of the alliance the client joined.
        /// </summary>
        public string Name;
        /// <summary>
        /// Badge of the alliance the client joined.
        /// </summary>
        public int Badge;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;

        /// <summary>
        /// Level of the alliance the client joined.
        /// </summary>
        public int Level;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;

        /// <summary>
        /// Current tick.
        /// </summary>
        public int Tick; // -1

        /// <summary>
        /// Reads the <see cref="JoinAllianceCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="JoinAllianceCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ClanID = reader.ReadInt64();
            Name = reader.ReadString();
            Badge = reader.ReadInt32();

            Unknown1 = reader.ReadByte();

            Level = reader.ReadInt32();

            Unknown2 = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="JoinAllianceCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="JoinAllianceCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>

        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ClanID);
            writer.Write(Name);
            writer.Write(Badge);

            writer.Write(Unknown1);

            writer.Write(Level);

            writer.Write(Unknown2);

            writer.Write(Tick);
        }
    }
}
