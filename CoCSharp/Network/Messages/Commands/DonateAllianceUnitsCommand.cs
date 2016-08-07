using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DonateAllianceUnitCommand : Command
    {
        /// <summary>
        /// 
        /// </summary>
        public DonateAllianceUnitCommand()
        {
            // Space
        }
        /// <summary>
        /// 
        /// </summary>
        public override int ID
        {
           get
           {
                return 4;
           }
        }
        /// <summary>
        /// Player ID
        /// </summary>
        public uint PlayerId;

        /// <summary>
        /// Unit type
        /// </summary>
        public uint UnitType;
        /// <summary>
        /// Unit Type
        /// </summary>
        public uint SpellType;

        /// <summary>
        /// Unknown interger 1
        /// </summary>
        public uint Unknown1;
        /// <summary>
        /// Unknown interger 2
        /// </summary>
        public uint Unknown2;

        /// <summary>
        /// Reads the <see cref="DonateAllianceUnitCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="DonateAllianceUnitCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);
            PlayerId = reader.ReadUInt32();
            SpellType = reader.ReadUInt32();
            UnitType = reader.ReadUInt32();
            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();
        }
        /// <summary>
        /// Writes the <see cref="DonateAllianceUnitCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="DonateAllianceUnitCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
            writer.Write(PlayerId);
            writer.Write(SpellType);
            writer.Write(UnitType);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            
        }
    }
}
