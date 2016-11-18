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

        public override int ID => 600;

        public int X;
        public int Y;
        public int UnitDataID;

        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            UnitDataID = reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(UnitDataID);
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
            var slot = level.Avatar.Units.GetSlot(UnitDataID);
            if (slot != null)
                slot.Amount--;
        }
    }
}
