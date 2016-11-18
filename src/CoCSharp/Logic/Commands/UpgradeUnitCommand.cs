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

        public override int ID => 516;

        public int BuildingGameID;
        public int Unknown1;
        public int UnitDataID;

        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            BuildingGameID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            UnitDataID = reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(BuildingGameID);
            writer.Write(Unknown1);
            writer.Write(UnitDataID);
            writer.Write(Tick);
        }

        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);

            //TODO: Implement TickTiming and stuff.
            //TODO: Implement proper loading of data instead of checking ID range.

            // Spell.
            if (UnitDataID >= 26000000 && UnitDataID < 27000000)
            {
                var slot = level.Avatar.SpellUpgrades.GetSlot(UnitDataID);
                if (slot == null)
                    level.Avatar.SpellUpgrades.Add(new SpellUpgradeSlot(UnitDataID, 1));
                else
                    slot.Level++;
            }
            // Character.
            else if (UnitDataID >= 4000000 && UnitDataID < 5000000)
            {
                var slot = level.Avatar.UnitUpgrades.GetSlot(UnitDataID);
                if (slot == null)
                    level.Avatar.UnitUpgrades.Add(new UnitUpgradeSlot(UnitDataID, 1));
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
