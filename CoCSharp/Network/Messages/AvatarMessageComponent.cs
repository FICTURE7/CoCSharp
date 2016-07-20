using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents an <see cref="Avatar"/>'s data sent in the
    /// networking protocol.
    /// </summary>
    public class AvatarMessageComponent : MessageComponent
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarMessageComponent"/> class.
        /// </summary>
        public AvatarMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarMessageComponent"/> class from
        /// the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="avatar"><see cref="Avatar"/> from which the data will be set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="avatar"/> is null.</exception>
        public AvatarMessageComponent(Avatar avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            UserID = avatar.ID;
            HomeID = avatar.ID;

            if (avatar.Alliance != null)
            {
                ClanData = new ClanMessageComponent()
                {
                    ID = avatar.Alliance.ID,
                    Name = avatar.Alliance.Name,
                    Badge = avatar.Alliance.Badge,
                    Role = avatar.Alliance.Role,
                    Level = avatar.Alliance.Level
                };
            }

            LeagueLevel = avatar.League;
            //TownHallLevel = avatar.Home.TownHall.Level;
            Name = avatar.Name;

            //Unknown13 = -1;
            Unknown14 = -1;
            Unknown15 = -1;

            Experience = avatar.Experience;
            Level = avatar.Level;
            Gems = avatar.Gems;
            FreeGems = avatar.FreeGems;

            Unknown16 = 1200;
            Unknown17 = 60;

            Trophies = avatar.Trophies;

            Unknown21 = 1;
            Unknown22 = 946720861000;

            IsNamed = avatar.IsNamed;

            Unknown26 = 1;
            //Unknown27 = 1;

            ResourcesCapacity = avatar.ResourcesCapacity;
            ResourcesAmount = avatar.ResourcesAmount;
            Units = avatar.Units;
            Spells = avatar.Spells;
            UnitUpgrades = avatar.UnitUpgrades;
            SpellUpgrades = avatar.SpellUpgrades;
            HeroUpgrades = avatar.HeroUpgrades;
            HeroHealths = avatar.HeroHealths;
            HeroStates = avatar.HeroStates;
            TutorialProgess = avatar.TutorialProgess;
            Achievements = avatar.Acheivements;
            AchievementProgress = avatar.AcheivementProgress;
            NpcStars = avatar.NpcStars;
            NpcGold = avatar.NpcGold;
            NpcElixir = avatar.NpcElixir;

            UnknownSlot1 = new UnknownSlot[0];
            UnknownSlot2 = new UnknownSlot[0];
            UnknownSlot3 = new UnknownSlot[0];
        }
        #endregion

        #region Fields
        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// User ID.
        /// </summary>
        public long UserID;
        /// <summary>
        /// Home ID. Usually same as <see cref="UserID"/>.
        /// </summary>
        public long HomeID;
        /// <summary>
        /// Data of the clan of the avatar.
        /// </summary>
        public ClanMessageComponent ClanData;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2; // 0
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3; // 0
        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4; // 0
        /// <summary>
        /// Unknown integer 5.
        /// </summary>
        public int Unknown5; // 0
        /// <summary>
        /// Unknown integer 6.
        /// </summary>
        public int Unknown6; // 0
        /// <summary>
        /// Unknown integer 7.
        /// </summary>
        public int Unknown7; // 0
        /// <summary>
        /// Unknown integer 8.
        /// </summary>
        public int Unknown8; // 0
        /// <summary>
        /// Unknown integer 9.
        /// </summary>
        public int Unknown9; // 0
        /// <summary>
        /// Unknown integer 10.
        /// </summary>
        public int Unknown10; // 0
        /// <summary>
        /// Unknown integer 11.
        /// </summary>
        public int Unknown11; // 0
        /// <summary>
        /// Unknown integer 12.
        /// </summary>
        public int Unknown12; // 0
        /// <summary>
        /// Unknown integer 13.
        /// </summary>
        public int Unknown13; // -1

        /// <summary>
        /// League ID.
        /// </summary>
        public int LeagueLevel;
        /// <summary>
        /// Alliance castle level.
        /// </summary>
        public int AllianceCastleLevel;
        /// <summary>
        /// Alliance castle capacity.
        /// </summary>
        public int AllianceCastleTotalCapacity;
        /// <summary>
        /// Alliance castle troop count.
        /// </summary>
        public int AllianceCastleUsedCapacity;
        /// <summary>
        /// TownHall level.
        /// </summary>
        public int TownHallLevel;
        /// <summary>
        /// Name of avatar.
        /// </summary>
        public string Name;

        /// <summary>
        /// Unknown integer 14.
        /// </summary>
        public int Unknown14; // -1

        /// <summary>
        /// Level of avatar.
        /// </summary>
        public int Level;
        /// <summary>
        /// Experience of avatar.
        /// </summary>
        public int Experience;
        /// <summary>
        /// Gems available.
        /// </summary>
        public int Gems;
        /// <summary>
        /// Free gems available.
        /// </summary>
        public int FreeGems;

        /// <summary>
        /// Unknown integer 15.
        /// </summary>
        public int Unknown15;
        /// <summary>
        /// Unknown integer 16.
        /// </summary>
        public int Unknown16;

        /// <summary>
        /// Trophies count.
        /// </summary>
        public int Trophies;

        /// <summary>
        /// Number of attacks won.
        /// </summary>
        public int AttacksWon; // 0
        /// <summary>
        /// Number of attacks lost.
        /// </summary>
        public int AttacksLost; // 0
        /// <summary>
        /// Number of defenses won.
        /// </summary>
        public int DefensesWon; // 0
        /// <summary>
        /// Number of defenses lost.
        /// </summary> 
        public int DefensesLost; // 0

        /// <summary>
        /// Unknown integer 17.
        /// </summary>
        public int Unknown17; // 0
        /// <summary>
        /// Unknown integer 18.
        /// </summary>
        public int Unknown18; // 0
        /// <summary>
        /// Unknown integer 19.
        /// </summary>
        public int Unknown19; // 0
        /// <summary>
        /// Unknown integer 20.
        /// </summary>
        public int Unknown20; // 1
        /// <summary>
        /// Unknown byte 21.
        /// </summary>
        public byte Unknown21;

        /// <summary>
        /// Avatar name is set.
        /// </summary>
        public bool IsNamed;

        /// <summary>
        /// Unknown long 22.
        /// </summary>
        public long Unknown22;
        /// <summary>
        /// Unknown integer 23.
        /// </summary>
        public int Unknown23;
        /// <summary>
        /// Unknown integer 24.
        /// </summary>
        public int Unknown24;
        /// <summary>
        /// Unknown integer 25.
        /// </summary>
        public int Unknown25;
        /// <summary>
        /// Unknown integer 26.
        /// </summary>
        public int Unknown26;
        /// <summary>
        /// Unknown integer 27.
        /// </summary>
        public int Unknown27;
        /// <summary>
        /// Unknown integer 28.
        /// </summary>
        public int Unknown28;

        /// <summary>
        /// Resources capacity.
        /// </summary>
        public ResourceCapacitySlot[] ResourcesCapacity;
        /// <summary>
        /// Resources amount.
        /// </summary>
        public ResourceAmountSlot[] ResourcesAmount;
        /// <summary>
        /// Units.
        /// </summary>
        public UnitSlot[] Units;
        /// <summary>
        /// Spells.
        /// </summary>
        public SpellSlot[] Spells;
        /// <summary>
        /// Unit upgrades level.
        /// </summary>
        public UnitUpgradeSlot[] UnitUpgrades;
        /// <summary>
        /// Spell upgrades level.
        /// </summary>
        public SpellUpgradeSlot[] SpellUpgrades;
        /// <summary>
        /// Hero upgrades level.
        /// </summary>
        public HeroUpgradeSlot[] HeroUpgrades;
        /// <summary>
        /// Hero healths.
        /// </summary>
        public HeroHealthSlot[] HeroHealths;
        /// <summary>
        /// Hero states.
        /// </summary>
        public HeroStateSlot[] HeroStates;
        /// <summary>
        /// Alliance units
        /// </summary>
        public AllianceUnitSlot[] AllianceUnits;
        /// <summary>
        /// Tutorial progress.
        /// </summary>
        public TutorialProgressSlot[] TutorialProgess;
        /// <summary>
        /// Achievements state.
        /// </summary>
        public AchievementSlot[] Achievements;
        /// <summary>
        /// Achievement progress.
        /// </summary>
        public AchievementProgessSlot[] AchievementProgress;
        /// <summary>
        /// NPC stars.
        /// </summary>
        public NpcStarSlot[] NpcStars;
        /// <summary>
        /// NPC gold.
        /// </summary>
        public NpcGoldSlot[] NpcGold;
        /// <summary>
        /// NPC elixir.
        /// </summary>
        public NpcElixirSlot[] NpcElixir;

        /// <summary>
        /// Unknown integer 29.
        /// </summary>
        public int Unknown29;

        /// <summary>
        /// Unknown slot 1.
        /// </summary>
        public UnknownSlot[] UnknownSlot1;
        /// <summary>
        /// Unknown slot 2.
        /// </summary>
        public UnknownSlot[] UnknownSlot2;
        /// <summary>
        /// Unknown slot 3.
        /// </summary>
        public UnknownSlot[] UnknownSlot3;
        #endregion

        #region Methods
        /// <summary>
        /// Reads the <see cref="AvatarMessageComponent"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32();

            UserID = reader.ReadInt64();
            HomeID = reader.ReadInt64();
            if (reader.ReadBoolean())
            {
                ClanData = new ClanMessageComponent();
                ClanData.ID = reader.ReadInt64();
                ClanData.Name = reader.ReadString();
                ClanData.Badge = reader.ReadInt32();
                ClanData.Role = reader.ReadInt32();
                ClanData.Level = reader.ReadInt32();
                ClanData.Unknown1 = reader.ReadByte(); // Clan war?
                if (ClanData.Unknown1 == 1)
                    ClanData.Unknown2 = reader.ReadInt64();
            }

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            Unknown9 = reader.ReadInt32();
            Unknown10 = reader.ReadInt32();
            Unknown11 = reader.ReadInt32();
            Unknown12 = reader.ReadInt32();

            Unknown13 = reader.ReadInt32(); // 0 = 8.x.x
            Unknown14 = reader.ReadInt32(); // -1 = 8.x.x

            LeagueLevel = reader.ReadInt32();
            AllianceCastleLevel = reader.ReadInt32();
            AllianceCastleTotalCapacity = reader.ReadInt32();
            AllianceCastleUsedCapacity = reader.ReadInt32();
            TownHallLevel = reader.ReadInt32();
            Name = reader.ReadString();

            Unknown15 = reader.ReadInt32(); // -1, Facebook ID

            Level = reader.ReadInt32();
            Experience = reader.ReadInt32();
            Gems = reader.ReadInt32(); // Scrambled when not own avatar data.
            FreeGems = reader.ReadInt32(); // Scrambled when not own avatar data.

            Unknown16 = reader.ReadInt32(); // 1200 // Scrambled when not own avatar data.
            Unknown17 = reader.ReadInt32(); // 60 // Scrambled when not own avatar data.

            Trophies = reader.ReadInt32();
            AttacksWon = reader.ReadInt32();
            AttacksLost = reader.ReadInt32(); // Scrambled when not own avatar data.
            DefensesWon = reader.ReadInt32();
            DefensesLost = reader.ReadInt32(); // Scrambled when not own avatar data.

            Unknown18 = reader.ReadInt32();
            Unknown19 = reader.ReadInt32();
            Unknown20 = reader.ReadInt32();
            Unknown21 = reader.ReadByte(); // 1, might be a bool
            Unknown22 = reader.ReadInt64(); // 946720861000

            IsNamed = reader.ReadBoolean();

            Unknown23 = reader.ReadInt32();
            Unknown24 = reader.ReadInt32(); // Scrambled when not own avatar data.
            Unknown25 = reader.ReadInt32();
            Unknown26 = reader.ReadInt32(); // 1
            Unknown27 = reader.ReadInt32(); // 0 = 8.x.x
            Unknown28 = reader.ReadInt32(); // 0 = 8.x.x

            ResourcesCapacity = Slot.ReadSlotArray<ResourceCapacitySlot>(reader);
            ResourcesAmount = Slot.ReadSlotArray<ResourceAmountSlot>(reader);
            Units = Slot.ReadSlotArray<UnitSlot>(reader);
            Spells = Slot.ReadSlotArray<SpellSlot>(reader);
            UnitUpgrades = Slot.ReadSlotArray<UnitUpgradeSlot>(reader);
            SpellUpgrades = Slot.ReadSlotArray<SpellUpgradeSlot>(reader);
            HeroUpgrades = Slot.ReadSlotArray<HeroUpgradeSlot>(reader);
            HeroHealths = Slot.ReadSlotArray<HeroHealthSlot>(reader);
            HeroStates = Slot.ReadSlotArray<HeroStateSlot>(reader);
            AllianceUnits = Slot.ReadSlotArray<AllianceUnitSlot>(reader);
            TutorialProgess = Slot.ReadSlotArray<TutorialProgressSlot>(reader);
            Achievements = Slot.ReadSlotArray<AchievementSlot>(reader);
            AchievementProgress = Slot.ReadSlotArray<AchievementProgessSlot>(reader);
            NpcStars = Slot.ReadSlotArray<NpcStarSlot>(reader);
            NpcGold = Slot.ReadSlotArray<NpcGoldSlot>(reader);
            NpcElixir = Slot.ReadSlotArray<NpcElixirSlot>(reader);

            Unknown29 = reader.ReadInt32();

            UnknownSlot1 = Slot.ReadSlotArray<UnknownSlot>(reader);
            UnknownSlot2 = Slot.ReadSlotArray<UnknownSlot>(reader);
            UnknownSlot3 = Slot.ReadSlotArray<UnknownSlot>(reader);
        }

        /// <summary>
        /// Writes the <see cref="AvatarMessageComponent"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);

            writer.Write(UserID);
            writer.Write(HomeID);
            writer.Write(ClanData != null);
            if (ClanData != null)
            {
                writer.Write(ClanData.ID);
                writer.Write(ClanData.Name);
                writer.Write(ClanData.Badge);
                writer.Write(ClanData.Role);
                writer.Write(ClanData.Level);
                writer.Write(ClanData.Unknown1);
                if (ClanData.Unknown1 == 1)
                    writer.Write(ClanData.Unknown2);
            }

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11); // 1
            writer.Write(Unknown12);
            writer.Write(Unknown13); // 1 = 8.x.x
            writer.Write(Unknown14); // 0 = 8.x.x

            writer.Write(LeagueLevel);
            writer.Write(AllianceCastleLevel);
            writer.Write(AllianceCastleTotalCapacity);
            writer.Write(AllianceCastleUsedCapacity);
            writer.Write(TownHallLevel);
            writer.Write(Name);

            writer.Write(Unknown15); // -1

            writer.Write(Level);
            writer.Write(Experience);
            writer.Write(Gems);
            writer.Write(FreeGems);

            writer.Write(Unknown16); // 1200
            writer.Write(Unknown17); // 60

            writer.Write(Trophies);

            writer.Write(AttacksWon);
            writer.Write(AttacksLost);
            writer.Write(DefensesWon);
            writer.Write(DefensesLost);

            writer.Write(Unknown18);
            writer.Write(Unknown19);
            writer.Write(Unknown20);
            writer.Write(Unknown21); // 1, might be a bool
            writer.Write(Unknown22); // 946720861000

            writer.Write(IsNamed);

            writer.Write(Unknown23);
            writer.Write(Unknown24);
            writer.Write(Unknown25);
            writer.Write(Unknown26); // 1
            writer.Write(Unknown27); // 1 = 8.x.x
            writer.Write(Unknown28); // 0 = 8.x.x

            Slot.WriteSlotArray(writer, ResourcesCapacity);
            Slot.WriteSlotArray(writer, ResourcesAmount);
            Slot.WriteSlotArray(writer, Units);
            Slot.WriteSlotArray(writer, Spells);
            Slot.WriteSlotArray(writer, UnitUpgrades);
            Slot.WriteSlotArray(writer, SpellUpgrades);
            Slot.WriteSlotArray(writer, HeroUpgrades);
            Slot.WriteSlotArray(writer, HeroHealths);
            Slot.WriteSlotArray(writer, HeroStates);
            Slot.WriteSlotArray(writer, AllianceUnits);
            Slot.WriteSlotArray(writer, TutorialProgess);
            Slot.WriteSlotArray(writer, Achievements);
            Slot.WriteSlotArray(writer, AchievementProgress);
            Slot.WriteSlotArray(writer, NpcStars);
            Slot.WriteSlotArray(writer, NpcGold);
            Slot.WriteSlotArray(writer, NpcElixir);

            writer.Write(Unknown29);

            Slot.WriteSlotArray(writer, UnknownSlot1);
            Slot.WriteSlotArray(writer, UnknownSlot2);
            Slot.WriteSlotArray(writer, UnknownSlot3);
        }
        #endregion
    }
}
