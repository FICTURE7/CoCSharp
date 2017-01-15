using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using System;
using System.Diagnostics;

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
        /// the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> from which the data will be set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        public AvatarMessageComponent(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            var avatar = level.Avatar;
            UserId = avatar.Id;
            HomeId = avatar.Id;

            if (avatar.Alliance != null)
            {
                var member = avatar.Alliance.Get(avatar.Id);
                Debug.Assert(member != null);
                if (member == null)
                    throw new InvalidOperationException($"Unable to find ClanMember {avatar.Id} in the Alliance {avatar.Alliance.Id}.");

                ClanData = new ClanMessageComponent
                {
                    Id = avatar.Alliance.Id,
                    Name = avatar.Alliance.Name,
                    Badge = avatar.Alliance.Badge,
                    Role = member.Role,
                    Level = avatar.Alliance.ExpLevels
                };
            }
            else
            {
                AllianceCastleLevel = -1;
            }

            LeagueLevel = avatar.League;
            if (level.Village.TownHall == null)
            {
                level.Logs.Log("TownHall reference was null; using TownHallLevel 5.");
                TownHallLevel = 5;
            }
            else
            {
                TownHallLevel = level.Village.TownHall.UpgradeLevel;
            }
            Name = avatar.Name;

            //Unknown13 = -1;
            //Unknown14 = -1;
            Unknown15 = -1;

            ExpPoints = avatar.ExpPoints;
            ExpLevels = avatar.ExpLevels;
            Gems = avatar.Gems;
            FreeGems = avatar.FreeGems;

            Unknown16 = 1200;
            Unknown17 = 60;

            Trophies = avatar.Trophies;

            Unknown21 = 1;
            Unknown22 = 946720861000;
            //Unknown22 = 4294967516;
            //Unknown23 = -169129983;

            IsNamed = avatar.IsNamed;

            Unknown26 = 1;
            //Unknown27 = 1;

            ResourcesAmount = avatar.ResourcesAmount;
            ResourcesCapacity = avatar.ResourcesCapacity;
            Units = avatar.Units;
            Spells = avatar.Spells;
            UnitUpgrades = avatar.UnitUpgrades;
            SpellUpgrades = avatar.SpellUpgrades;
            HeroUpgrades = avatar.HeroUpgrades;
            HeroHealths = avatar.HeroHealths;
            HeroStates = avatar.HeroStates;
            TutorialProgress = avatar.TutorialProgess;
            Achievements = avatar.Achievements;
            AchievementProgress = avatar.AchievementProgress;
            NpcStars = avatar.NpcStars;
            NpcGold = avatar.NpcGold;
            NpcElixir = avatar.NpcElixir;

            UnknownSlot1 = new SlotCollection<UnknownSlot>();
            UnknownSlot2 = new SlotCollection<UnknownSlot>();
            UnknownSlot3 = new SlotCollection<UnknownSlot>();
            UnknownSlot4 = new SlotCollection<UnknownSlot>();
            UnknownSlot5 = new SlotCollection<UnknownSlot>();
            UnknownSlot6 = new SlotCollection<UnknownSlot>();
            UnknownSlot7 = new SlotCollection<UnitSlot>();
        }
        #endregion

        #region Fields
        /// <summary>
        /// User ID.
        /// </summary>
        public long UserId;
        /// <summary>
        /// Home ID. Usually same as <see cref="UserId"/>.
        /// </summary>
        public long HomeId;
        /// <summary>
        /// Data of the clan of the avatar.
        /// </summary>
        public ClanMessageComponent ClanData;

        /// <summary>
        /// Legendary Trophy Count.
        /// </summary>
        public int LegendaryTrophy; // 0
        /// <summary>
        /// Best Season Is Player Legendary Stats.
        /// </summary>
        public int BestSeasonEnabled; // 0
        /// <summary>
        /// Best Season In Legendary Stats Month.
        /// </summary>
        public int BestSeasonMonth; // 0
        /// <summary>
        /// Best Season In Legendary Stats Month.
        /// </summary>
        public int BestSeasonYear; // 0
        /// <summary>
        /// Best Season In Legendary Stats Year.
        /// </summary>
        public int BestSeasonPosition; // 0
        /// <summary>
        /// Best Season In Legendary Stats Trophies.
        /// </summary>
        public int BestSeasonTrophies; // 0
        /// <summary>
        /// Last Season Is Player Legendary Stats.
        /// </summary>
        public int LastSeasonEnabled; // 0
        /// <summary>
        /// Last Season In Legendary Stats Month.
        /// </summary>
        public int LastSeasonMonth; // 0
        /// <summary>
        /// Last Season In Legendary Stats Year.
        /// </summary>
        public int LastSeasonYear; // 0
        /// <summary>
        /// Last Season In Legendary Stats Position.
        /// </summary>
        public int LastSeasonPosition; // 0
        /// <summary>
        /// Last Season In Legendary Stats Trophies.
        /// </summary>
        public int LastSeasonTrophies; // 0
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
        /// Unknown integer 13.
        /// </summary>
        public int Unknown13; // -1
        /// <summary>
        /// Unknown integer 14.
        /// </summary>
        public int Unknown14; // -1
        /// <summary>
        /// TownHall level.
        /// </summary>
        public int TownHallLevel;
        /// <summary>
        /// Name of avatar.
        /// </summary>
        public string Name;
        /// <summary>
        /// Unknown integer 15.
        /// </summary>
        public int Unknown15;
        /// <summary>
        /// Level of avatar.
        /// </summary>
        public int ExpLevels;
        /// <summary>
        /// Experience of avatar.
        /// </summary>
        public int ExpPoints;
        /// <summary>
        /// Gems available.
        /// </summary>
        public int Gems;
        /// <summary>
        /// Free gems available.
        /// </summary>
        public int FreeGems;

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
        /// Unknown integer 29.
        /// </summary>
        public int Unknown29;
        /// <summary>
        /// Unknown byte 30.
        /// </summary>
        public byte Unknown30;

        /// <summary>
        /// Resources capacity.
        /// </summary>
        public SlotCollection<ResourceCapacitySlot> ResourcesCapacity;
        /// <summary>
        /// Resources amount.
        /// </summary>
        public SlotCollection<ResourceAmountSlot> ResourcesAmount;
        /// <summary>
        /// Units.
        /// </summary>
        public SlotCollection<UnitSlot> Units;
        /// <summary>
        /// Spells.
        /// </summary>
        public SlotCollection<SpellSlot> Spells;
        /// <summary>
        /// Unit upgrades level.
        /// </summary>
        public SlotCollection<UnitUpgradeSlot> UnitUpgrades;
        /// <summary>
        /// Spell upgrades level.
        /// </summary>
        public SlotCollection<SpellUpgradeSlot> SpellUpgrades;
        /// <summary>
        /// Hero upgrades level.
        /// </summary>
        public SlotCollection<HeroUpgradeSlot> HeroUpgrades;
        /// <summary>
        /// Hero healths.
        /// </summary>
        public SlotCollection<HeroHealthSlot> HeroHealths;
        /// <summary>
        /// Hero states.
        /// </summary>
        public SlotCollection<HeroStateSlot> HeroStates;
        /// <summary>
        /// Alliance units
        /// </summary>
        public SlotCollection<AllianceUnitSlot> AllianceUnits;
        /// <summary>
        /// Tutorial progress.
        /// </summary>
        public SlotCollection<TutorialProgressSlot> TutorialProgress;
        /// <summary>
        /// Achievements state.
        /// </summary>
        public SlotCollection<AchievementSlot> Achievements;
        /// <summary>
        /// Achievement progress.
        /// </summary>
        public SlotCollection<AchievementProgessSlot> AchievementProgress;
        /// <summary>
        /// NPC stars.
        /// </summary>
        public SlotCollection<NpcStarSlot> NpcStars;
        /// <summary>
        /// NPC gold.
        /// </summary>
        public SlotCollection<NpcGoldSlot> NpcGold;
        /// <summary>
        /// NPC elixir.
        /// </summary>
        public SlotCollection<NpcElixirSlot> NpcElixir;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown31;

        /// <summary>
        /// Unknown slot 1.
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot1;
        /// <summary>
        /// Unknown slot 2.
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot2;
        /// <summary>
        /// Unknown slot 3.
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot3;
        /// <summary>
        /// Unknown slot 4.
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot4;
        /// <summary>
        /// Unknown slot 5;
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot5;
        /// <summary>
        /// Unknown slot 6;
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot6;
        /// <summary>
        /// Unknown slot 7;
        /// </summary>
        public SlotCollection<UnitSlot> UnknownSlot7;
        /// <summary>
        /// Unknown slot 8;
        /// </summary>
        public SlotCollection<UnknownSlot> UnknownSlot8;
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

            UserId = reader.ReadInt64();
            HomeId = reader.ReadInt64();
            if (reader.ReadBoolean())
            {
                ClanData = new ClanMessageComponent();
                ClanData.Id = reader.ReadInt64();
                ClanData.Name = reader.ReadString();
                ClanData.Badge = reader.ReadInt32();
                ClanData.Role = (ClanMemberRole)reader.ReadInt32();
                ClanData.Level = reader.ReadInt32();
                ClanData.Unknown1 = reader.ReadByte(); // Clan war?
                if (ClanData.Unknown1 == 1)
                    ClanData.Unknown2 = reader.ReadInt64(); // Clan war ID?
            }

            LegendaryTrophy = reader.ReadInt32();
            BestSeasonEnabled = reader.ReadInt32();
            BestSeasonMonth = reader.ReadInt32();
            BestSeasonYear = reader.ReadInt32();
            BestSeasonPosition = reader.ReadInt32();
            BestSeasonTrophies = reader.ReadInt32();
            LastSeasonEnabled = reader.ReadInt32();
            LastSeasonMonth = reader.ReadInt32();
            LastSeasonYear = reader.ReadInt32();
            LastSeasonPosition = reader.ReadInt32();
            LastSeasonTrophies = reader.ReadInt32();

            LeagueLevel = reader.ReadInt32();
            AllianceCastleLevel = reader.ReadInt32();
            AllianceCastleTotalCapacity = reader.ReadInt32();
            AllianceCastleUsedCapacity = reader.ReadInt32();

            Unknown13 = reader.ReadInt32(); // 0 = 8.x.x
            Unknown14 = reader.ReadInt32(); // -1 = 8.x.x

            TownHallLevel = reader.ReadInt32();
            Name = reader.ReadString();

            Unknown15 = reader.ReadInt32(); // -1, Facebook ID

            ExpLevels = reader.ReadInt32();
            ExpPoints = reader.ReadInt32();
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

            // 8.511.4
            Unknown29 = reader.ReadInt32();

            Unknown21 = reader.ReadByte(); // 1, might be a bool
            Unknown22 = reader.ReadInt64(); // 946720861000

            IsNamed = reader.ReadBoolean();

            Unknown23 = reader.ReadInt32();
            Unknown24 = reader.ReadInt32(); // Scrambled when not own avatar data.
            Unknown25 = reader.ReadInt32();
            Unknown26 = reader.ReadInt32(); // 1
            Unknown27 = reader.ReadInt32(); // 0 = 8.x.x
            Unknown28 = reader.ReadInt32(); // 0 = 8.x.x

            // 8.551.4
            Unknown30 = reader.ReadByte();

            ResourcesCapacity = reader.ReadSlotCollection<ResourceCapacitySlot>();
            ResourcesAmount = reader.ReadSlotCollection<ResourceAmountSlot>();
            Units = reader.ReadSlotCollection<UnitSlot>();
            Spells = reader.ReadSlotCollection<SpellSlot>();
            UnitUpgrades = reader.ReadSlotCollection<UnitUpgradeSlot>();
            SpellUpgrades = reader.ReadSlotCollection<SpellUpgradeSlot>();
            HeroUpgrades = reader.ReadSlotCollection<HeroUpgradeSlot>();
            HeroHealths = reader.ReadSlotCollection<HeroHealthSlot>();
            HeroStates = reader.ReadSlotCollection<HeroStateSlot>();
            AllianceUnits = reader.ReadSlotCollection<AllianceUnitSlot>();
            TutorialProgress = reader.ReadSlotCollection<TutorialProgressSlot>();
            Achievements = reader.ReadSlotCollection<AchievementSlot>();
            AchievementProgress = reader.ReadSlotCollection<AchievementProgessSlot>();
            NpcStars = reader.ReadSlotCollection<NpcStarSlot>();
            NpcGold = reader.ReadSlotCollection<NpcGoldSlot>();
            NpcElixir = reader.ReadSlotCollection<NpcElixirSlot>();

            Unknown31 = reader.ReadInt32();

            UnknownSlot1 = reader.ReadSlotCollection<UnknownSlot>();
            UnknownSlot2 = reader.ReadSlotCollection<UnknownSlot>();
            UnknownSlot3 = reader.ReadSlotCollection<UnknownSlot>();

            // 8.551.4
            UnknownSlot4 = reader.ReadSlotCollection<UnknownSlot>();
            UnknownSlot5 = reader.ReadSlotCollection<UnknownSlot>();
            UnknownSlot6 = reader.ReadSlotCollection<UnknownSlot>();
            UnknownSlot7 = reader.ReadSlotCollection<UnitSlot>();

            // 8.709.2
            // Could be an int32 instead.
            UnknownSlot8 = reader.ReadSlotCollection<UnknownSlot>();
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

            writer.Write(UserId);
            writer.Write(HomeId);
            writer.Write(ClanData != null);
            if (ClanData != null)
            {
                writer.Write(ClanData.Id);
                writer.Write(ClanData.Name);
                writer.Write(ClanData.Badge);
                writer.Write((int)ClanData.Role);
                writer.Write(ClanData.Level);
                writer.Write(ClanData.Unknown1);
                if (ClanData.Unknown1 == 1)
                    writer.Write(ClanData.Unknown2);
            }

            writer.Write(LegendaryTrophy);
            writer.Write(BestSeasonEnabled);
            writer.Write(BestSeasonMonth);
            writer.Write(BestSeasonYear);
            writer.Write(BestSeasonPosition);
            writer.Write(BestSeasonTrophies);
            writer.Write(LastSeasonEnabled);
            writer.Write(LastSeasonMonth);
            writer.Write(LastSeasonYear);
            writer.Write(LastSeasonPosition); // 1
            writer.Write(LastSeasonTrophies);

            writer.Write(LeagueLevel);
            writer.Write(AllianceCastleLevel);
            writer.Write(AllianceCastleTotalCapacity);
            writer.Write(AllianceCastleUsedCapacity);
            writer.Write(Unknown13); // 1 = 8.x.x
            writer.Write(Unknown14); // 0 = 8.x.x
            writer.Write(TownHallLevel);
            writer.Write(Name);

            writer.Write(Unknown15); // -1

            writer.Write(ExpLevels);
            writer.Write(ExpPoints);
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

            // 8.551.4
            writer.Write(Unknown29);

            writer.Write(Unknown21); // 1, might be a bool
            writer.Write(Unknown22); // 946720861000

            writer.Write(IsNamed);

            writer.Write(Unknown23);
            writer.Write(Unknown24);
            writer.Write(Unknown25);
            writer.Write(Unknown26); // 1
            writer.Write(Unknown27); // 1 = 8.x.x
            writer.Write(Unknown28); // 0 = 8.x.x

            // 8.551.4
            writer.Write(Unknown30);

            writer.WriteSlotCollection(ResourcesCapacity);
            writer.WriteSlotCollection(ResourcesAmount);
            writer.WriteSlotCollection(Units);
            writer.WriteSlotCollection(Spells);
            writer.WriteSlotCollection(UnitUpgrades);
            writer.WriteSlotCollection(SpellUpgrades);
            writer.WriteSlotCollection(HeroUpgrades);
            writer.WriteSlotCollection(HeroHealths);
            writer.WriteSlotCollection(HeroStates);
            writer.WriteSlotCollection(AllianceUnits);
            writer.WriteSlotCollection(TutorialProgress);
            writer.WriteSlotCollection(Achievements);
            writer.WriteSlotCollection(AchievementProgress);
            writer.WriteSlotCollection(NpcStars);
            writer.WriteSlotCollection(NpcGold);
            writer.WriteSlotCollection(NpcElixir);

            writer.Write(Unknown31);

            writer.WriteSlotCollection(UnknownSlot1);
            writer.WriteSlotCollection(UnknownSlot2);
            writer.WriteSlotCollection(UnknownSlot3);
            writer.WriteSlotCollection(UnknownSlot4);
            writer.WriteSlotCollection(UnknownSlot5);
            writer.WriteSlotCollection(UnknownSlot6);
            writer.WriteSlotCollection(UnknownSlot7);

            writer.WriteSlotCollection(UnknownSlot8);
        }
        #endregion
    }
}
