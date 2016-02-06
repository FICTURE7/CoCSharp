using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Networking;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents an <see cref="Avatar"/>'s data sent in the
    /// networking protocol.
    /// </summary>
    public class AvatarData
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarData"/> class.
        /// </summary>
        public AvatarData()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarData"/> class from
        /// the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="avatar"><see cref="Avatar"/> from which the data will be set.</param>
        public AvatarData(Avatar avatar)
        {
            OwnVillageData = new VillageData()
            {
                UserID = avatar.ID,
                ShieldDuration = avatar.ShieldDuration,
                Unknown2 = 1200,
                Unknown3 = 60,
                Compressed = true,
                Home = avatar.Home,
            };

            UserID = avatar.ID;
            UserID2 = avatar.ID; //TODO: Might be something different.

            if (avatar.Alliance != null)
            {
                OwnClanData = new ClanData()
                {
                    ID = avatar.Alliance.ID,
                    Name = avatar.Alliance.Name,
                    Badge = avatar.Alliance.Badge,
                    Role = avatar.Alliance.Role,
                    Level = avatar.Alliance.Level
                };
            }

            League = avatar.League;
            Name = avatar.Name;

            Unknown13 = -1;
            Unknown14 = -1;

            Experience = avatar.Experience;
            Level = avatar.Level;
            Gems = avatar.Gems;
            FreeGems = avatar.FreeGems;

            Unknown15 = 1200;
            Unknown16 = 60;

            Trophies = avatar.Trophies;

            Unknown20 = 1;
            Unknown21 = 946720861000;

            IsNamed = avatar.IsNamed;

            Unknown25 = 1;
            Unknown26 = 1;

            ResourcesCapacity = new ResourceCapacitySlot[0];
            ResourcesAmount = new ResourceAmountSlot[0];
            Units = new UnitSlot[0];
            Spells = new SpellSlot[0];
            UnitUpgrades = new UnitUpgradeSlot[0];
            SpellUpgrades = new SpellUpgradeSlot[0];
            HeroUpgrades = new HeroUpgradeSlot[0];
            HeroHealths = new HeroHealthSlot[0];
            HeroStates = new HeroStateSlot[0];
            TutorialProgess = new TutorialProgressSlot[0];
            Acheivements = new AchievementSlot[0];
            AcheivementProgress = new AchievementProgessSlot[0];
            NpcStars = new NpcStarSlot[0];
            NpcGold = new NpcGoldSlot[0];
            NpcElixir = new NpcElixirSlot[0];
            UnknownSlot1 = new UnknownSlot[0];
            UnknownSlot2 = new UnknownSlot[0];
            UnknownSlot3 = new UnknownSlot[0];
            UnknownSlot4 = new UnknownSlot[0];

            Unknown30 = 1454783074000;
            Unknown31 = 1454783074000;
            Unknown32 = 1454783074000;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Data of the village of the avatar.
        /// </summary>
        public VillageData OwnVillageData;
        /// <summary>
        /// User ID.
        /// </summary>
        public long UserID;
        /// <summary>
        /// User ID. Might be current home ID.
        /// </summary>
        public long UserID2;
        /// <summary>
        /// Data of the clan of the avatar.
        /// </summary>
        public ClanData OwnClanData;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;
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
        public int League;
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
        public int Unknown15; // 1200
        /// <summary>
        /// Unknown integer 16.
        /// </summary>
        public int Unknown16; // 60
        /// <summary>
        /// Score.
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
        /// Unknown byte 20.
        /// </summary>
        public byte Unknown20; // 1
        /// <summary>
        /// Unknown long 21.
        /// </summary>
        public long Unknown21;
        /// <summary>
        /// Avatar name is set.
        /// </summary>
        public bool IsNamed;
        /// <summary>
        /// Unknown integer 22.
        /// </summary>
        public int Unknown22;
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
        /// Acheivements state.
        /// </summary>
        public AchievementSlot[] Acheivements;
        /// <summary>
        /// Acheivement progress.
        /// </summary>
        public AchievementProgessSlot[] AcheivementProgress;
        /// <summary>
        /// Npc stars.
        /// </summary>
        public NpcStarSlot[] NpcStars;
        /// <summary>
        /// Npc gold.
        /// </summary>
        public NpcGoldSlot[] NpcGold;
        /// <summary>
        /// Npc elixir.
        /// </summary>
        public NpcElixirSlot[] NpcElixir;

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
        /// <summary>
        /// Unknown slot 4.
        /// </summary>
        public UnknownSlot[] UnknownSlot4;

        /// <summary>
        /// Unknown integer 28.
        /// </summary>
        public int Unknown28;
        /// <summary>
        /// Unknown integer 29.
        /// </summary>
        public int Unknown29;
        /// <summary>
        /// Unknown long 30.
        /// </summary>
        public long Unknown30;
        /// <summary>
        /// Unknown long 31
        /// </summary>
        public long Unknown31;
        /// <summary>
        /// Unknown long 32.
        /// </summary>
        public long Unknown32;
        #endregion

        #region Methods
        /// <summary>
        /// Reads the <see cref="AvatarData"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarData"/>.
        /// </param>
        public void Read(MessageReader reader)
        {
            OwnVillageData = new VillageData();
            OwnVillageData.Read(reader);

            UserID = reader.ReadInt64();
            UserID2 = reader.ReadInt64();
            OwnClanData = new ClanData();
            if (reader.ReadBoolean())
            {
                OwnClanData.ID = reader.ReadInt64();
                OwnClanData.Name = reader.ReadString();
                OwnClanData.Badge = reader.ReadInt32();
                OwnClanData.Role = reader.ReadInt32();
                OwnClanData.Level = reader.ReadInt32();
                OwnClanData.Unknown1 = reader.ReadByte();
            }

            Unknown1 = reader.ReadInt32();
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

            Unknown12 = reader.ReadInt32(); // 0 = 8.x.x
            Unknown13 = reader.ReadInt32(); // -1 = 8.x.x

            League = reader.ReadInt32();
            AllianceCastleLevel = reader.ReadInt32(); // might be at wrong offset
            AllianceCastleTotalCapacity = reader.ReadInt32();
            AllianceCastleUsedCapacity = reader.ReadInt32();
            TownHallLevel = reader.ReadInt32();
            Name = reader.ReadString();

            Unknown14 = reader.ReadInt32(); // -1, facebook ID

            Level = reader.ReadInt32();
            Experience = reader.ReadInt32();
            Gems = reader.ReadInt32();
            FreeGems = reader.ReadInt32();

            Unknown15 = reader.ReadInt32(); // 1200
            Unknown16 = reader.ReadInt32(); // 60

            Trophies = reader.ReadInt32();

            AttacksWon = reader.ReadInt32();
            AttacksLost = reader.ReadInt32();
            DefensesWon = reader.ReadInt32();
            DefensesLost = reader.ReadInt32();

            Unknown17 = reader.ReadInt32();
            Unknown18 = reader.ReadInt32();
            Unknown19 = reader.ReadInt32();
            Unknown20 = reader.ReadByte(); // 1, might be a bool
            Unknown21 = reader.ReadInt64(); // 946720861000

            IsNamed = reader.ReadBoolean();

            Unknown22 = reader.ReadInt32();
            Unknown23 = reader.ReadInt32();
            Unknown24 = reader.ReadInt32();
            Unknown25 = reader.ReadInt32(); // 1

            Unknown26 = reader.ReadInt32(); // 1 = 8.x.x
            Unknown27 = reader.ReadInt32(); // 0 = 8.x.x

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
            Acheivements = Slot.ReadSlotArray<AchievementSlot>(reader);
            AcheivementProgress = Slot.ReadSlotArray<AchievementProgessSlot>(reader);
            NpcStars = Slot.ReadSlotArray<NpcStarSlot>(reader);
            NpcGold = Slot.ReadSlotArray<NpcGoldSlot>(reader);
            NpcElixir = Slot.ReadSlotArray<NpcElixirSlot>(reader);

            UnknownSlot1 = Slot.ReadSlotArray<UnknownSlot>(reader);
            UnknownSlot2 = Slot.ReadSlotArray<UnknownSlot>(reader);
            UnknownSlot3 = Slot.ReadSlotArray<UnknownSlot>(reader);
            UnknownSlot4 = Slot.ReadSlotArray<UnknownSlot>(reader); // ID = 37000002, value = 0

            Unknown28 = reader.ReadInt32(); // 0
            Unknown29 = reader.ReadInt32(); // 0
            Unknown30 = reader.ReadInt64(); // 1454781274000
            Unknown31 = reader.ReadInt64(); // 1454781274000
            Unknown32 = reader.ReadInt64(); // 1454783074000
        }

        /// <summary>
        /// Writes the <see cref="AvatarData"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarData"/>.
        /// </param>
        public void Write(MessageWriter writer)
        {
            if (OwnVillageData == null)
                throw new NullReferenceException("OwnAvatarData was null.");

            OwnVillageData.Write(writer);

            writer.Write(UserID);
            writer.Write(UserID2);
            writer.Write(OwnClanData != null);
            if (OwnClanData != null)
            {
                writer.Write(OwnClanData.ID);
                writer.Write(OwnClanData.Name);
                writer.Write(OwnClanData.Badge);
                writer.Write(OwnClanData.Role);
                writer.Write(OwnClanData.Level);
                writer.Write(OwnClanData.Unknown1);
            }

            writer.Write(Unknown1);
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

            writer.Write(Unknown12); // 1 = 8.x.x
            writer.Write(Unknown13); // 0 = 8.x.x

            writer.Write(League);
            writer.Write(AllianceCastleLevel);
            writer.Write(AllianceCastleTotalCapacity);
            writer.Write(AllianceCastleUsedCapacity);
            writer.Write(TownHallLevel);
            writer.Write(Name);

            writer.Write(Unknown13); // -1

            writer.Write(Level);
            writer.Write(Experience);
            writer.Write(Gems);
            writer.Write(FreeGems);

            writer.Write(Unknown14); // 1200
            writer.Write(Unknown15); // 60

            writer.Write(Trophies);

            writer.Write(AttacksWon);
            writer.Write(AttacksLost);
            writer.Write(DefensesWon);
            writer.Write(DefensesLost);

            writer.Write(Unknown17);
            writer.Write(Unknown18);
            writer.Write(Unknown19);
            writer.Write(Unknown20); // 1, might be a bool
            writer.Write(Unknown21); // 946720861000

            writer.Write(IsNamed);

            writer.Write(Unknown22);
            writer.Write(Unknown23);
            writer.Write(Unknown24);
            writer.Write(Unknown25); // 1

            writer.Write(Unknown26); // 1 = 8.x.x
            writer.Write(Unknown27); // 0 = 8.x.x

            Slot.WriteSlotArray(writer, ResourcesCapacity);
            Slot.WriteSlotArray(writer, ResourcesAmount);
            Slot.WriteSlotArray(writer, Units);
            Slot.WriteSlotArray(writer, Spells);
            Slot.WriteSlotArray(writer, UnitUpgrades);
            Slot.WriteSlotArray(writer, SpellUpgrades);
            Slot.WriteSlotArray(writer, HeroUpgrades);
            Slot.WriteSlotArray(writer, HeroHealths);
            Slot.WriteSlotArray(writer, HeroStates);
            Slot.WriteSlotArray(writer, TutorialProgess);
            Slot.WriteSlotArray(writer, Acheivements);
            Slot.WriteSlotArray(writer, AcheivementProgress);
            Slot.WriteSlotArray(writer, NpcStars);
            Slot.WriteSlotArray(writer, NpcGold);
            Slot.WriteSlotArray(writer, NpcElixir);
            Slot.WriteSlotArray(writer, UnknownSlot1);
            Slot.WriteSlotArray(writer, UnknownSlot2);
            Slot.WriteSlotArray(writer, UnknownSlot3);
            Slot.WriteSlotArray(writer, UnknownSlot4);

            writer.Write(Unknown28);
            writer.Write(Unknown29);
            writer.Write(Unknown30);
            writer.Write(Unknown31);
            writer.Write(Unknown32);
        }
        #endregion
    }
}
