using CoCSharp.Network;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
{
    public class PlaceAttackerCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceAttackerCommand"/> class.
        /// </summary>
        public PlaceAttackerCommand()
        {
            // Space
        }

        public override int Id => 600;

        public int X;
        public int Y;
        public int UnitDataId;

        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            UnitDataId = reader.ReadInt32();
            Tick = reader.ReadInt32();

            //var nx = (X >> 8) / 2;
            //var ny = (Y >> 8) / 2;
        }

        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(UnitDataId);
            writer.Write(Tick);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);

            Debug.Assert(level.State >= 1, "Command was executed when the level was in an incorrect state.");

            // State == 2, means we're attacking the player.
            level.State = 2;

            // Look the UnitSlot in the avatar's UnitSlotCollection and
            // decrement its amount.
            var slot = level.Avatar.Units.GetSlot(UnitDataId);
            if (slot != null)
                slot.Amount--;
        }
    }
}
