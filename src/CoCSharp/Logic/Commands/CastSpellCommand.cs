using CoCSharp.Network;

namespace CoCSharp.Logic.Commands
{
    public class CastSpellCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CastSpellCommand"/> class.
        /// </summary>
        public CastSpellCommand()
        {
            // Space
        }

        public override int Id => 604;

        public int X;
        public int Y;
        public int SpellDataID;

        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            SpellDataID = reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(SpellDataID);
            writer.Write(Tick);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);

            //TODO: Implement TickTiming and stuff.
            var slot = level.Avatar.Spells.GetSlot(SpellDataID);
            if (slot != null)
                slot.Amount--;
        }
    }
}
