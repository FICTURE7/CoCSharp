using CoCSharp.Data.Slots;
using CoCSharp.Network;

namespace CoCSharp.Logic.Commands
{
    public class UpgradeUnitCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeUnitCommand"/> class.
        /// </summary>
        public UpgradeUnitCommand()
        {
            // Space
        }

        public override int Id => 516;

        public int BuildingGameID;
        public int Unknown1;
        public int UnitDataId;

        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildingGameID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            UnitDataId = reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildingGameID);
            writer.Write(Unknown1);
            writer.Write(UnitDataId);
            writer.Write(Tick);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);

            //TODO: Implement TickTiming and stuff.
            //TODO: Implement proper loading of data instead of checking ID range.

            // Spell.
            if (UnitDataId >= 26000000 && UnitDataId < 27000000)
            {
                var slot = level.Avatar.SpellUpgrades.GetSlot(UnitDataId);
                if (slot == null)
                    level.Avatar.SpellUpgrades.Add(new SpellUpgradeSlot(UnitDataId, 1));
                else
                    slot.Level++;
            }
            // Character.
            else if (UnitDataId >= 4000000 && UnitDataId < 5000000)
            {
                var slot = level.Avatar.UnitUpgrades.GetSlot(UnitDataId);
                if (slot == null)
                    level.Avatar.UnitUpgrades.Add(new UnitUpgradeSlot(UnitDataId, 1));
                else
                    slot.Level++;
            }
            else
            {
                level.Logs.Log("Unexpected Unit data type.");
            }
        }
    }
}
