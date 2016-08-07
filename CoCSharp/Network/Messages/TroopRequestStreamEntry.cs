using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// TroopsRequestStreamEntry
    /// </summary>
    public class TroopRequestStreamEntry : StreamEntry
    {
        /// <summary>
        /// Troops Req stream entry
        /// </summary>
        public TroopRequestStreamEntry()
        {
            // Space
        }
        /// <summary>
        /// Get the ID of the <see cref="TroopRequestStreamEntry"/>
        /// </summary>
        public override int ID
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        /// Max troops you can donate
        /// </summary>
        public int MaxTroops;
        /// <summary>
        /// Max spells you can donate
        /// </summary>
        
        public int MaxSpells;
        /// <summary>
        /// Player Donated troops
        /// </summary>
        public int DonatedTroops;
        /// <summary>
        /// Player Donated spells
        /// </summary>
        public int DonatedSpell;
        /// <summary>
        /// Message use to req
        /// </summary>
        public string Message;
        /// <summary>
        /// Reads the <see cref="TroopRequestStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="TroopRequestStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            ThrowIfReaderNull(reader);
            MaxTroops = reader.ReadInt32();
            MaxSpells = reader.ReadInt32();
            DonatedTroops = reader.ReadInt32();
            DonatedSpell = reader.ReadInt32();
            Message = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="TroopRequestStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="TroopRequestStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
            writer.Write(Message);
            writer.Write(MaxSpells);
            writer.Write(MaxTroops);
            writer.Write(DonatedSpell);
            writer.Write(DonatedTroops);
            
        }
    }
}
