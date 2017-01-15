using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the server to the client
    /// to tell it that it changed alliance setting.
    /// </summary>
    public class AllianceSettingChangedCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceSettingChangedCommand"/> class.
        /// </summary>
        public AllianceSettingChangedCommand()
        {
            //Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceSettingChangedCommand"/>.
        /// </summary>
        public override int Id { get { return 6; } }

        /// <summary>
        /// ID of the alliance whose settings was changed.
        /// </summary>
        public long ClanID;
        /// <summary>
        /// Badge of the alliance whose settings was changed.
        /// </summary>
        public int Badge;
        /// <summary>
        /// Level of the alliance whose settings was changed.
        /// </summary>
        public int Level;
        /// <summary>
        /// Current tick.
        /// </summary>
        public int Tick; // -1

        /// <summary>
        /// Reads the <see cref="AllianceSettingChangedCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceSettingChangedCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ClanID = reader.ReadInt64();
            Badge = reader.ReadInt32();
            Level = reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AllianceSettingChangedCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceSettingChangedCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ClanID);
            writer.Write(Badge);
            writer.Write(Level);
            writer.Write(Tick);
        }
    }
}
