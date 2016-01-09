using CoCSharp.Logic;
using CoCSharp.Networking;
using System;
using System.Collections.Generic;

namespace CoCSharp.Data
{
    //TODO: Make this piece of shit fancier.
    //TODO: Find a better name for AvatarData, ClanData, VillageData and MoveVillageObjectData.

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

            Unknown12 = -1;

            Experience = avatar.Experience;
            Level = avatar.Level;
            Gems = avatar.Gems;
            FreeGems = avatar.FreeGems;

            Unknown13 = 1200;
            Unknown14 = 60;

            Trophies = avatar.Trophies;

            Unknown22 = 1;
            Unknown23 = 946720861000;

            IsNamed = avatar.IsNamed;

            Unknown27 = 1;

            ResourceTotalCapacity = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            ResourceUsedCapacity = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            Units = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            Spells = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            UnitsUpgradeLevel = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            SpellsUpgradeLevel = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            HeroesUpgradeLevel = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            HeroesHealth = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            HeroesState = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            Tutorial = new AvatarData.SingleDataSlotList()
            {
                List = new List<AvatarData.SingleDataSlot>()
            };

            Acheivement = new AvatarData.SingleDataSlotList()
            {
                List = new List<AvatarData.SingleDataSlot>()
            };

            AcheivementProgress = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            NpcStars = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            NpcGold = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };

            NpcElixir = new AvatarData.DataSlotList()
            {
                List = new List<AvatarData.DataSlot>()
            };
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
        /// Unknown integer 12.
        /// </summary>
        public int Unknown12; // -1
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
        /// Unknown integer 13.
        /// </summary>
        public int Unknown13; // 1200
        /// <summary>
        /// Unknown integer 14.
        /// </summary>
        public int Unknown14; // 60
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
        /// Unknown integer 19.
        /// </summary>
        public int Unknown19; // 0
        /// <summary>
        /// Unknown integer 20.
        /// </summary>
        public int Unknown20; // 0
        /// <summary>
        /// Unknown integer 21.
        /// </summary>
        public int Unknown21; // 0
        /// <summary>
        /// Unknown byte 22.
        /// </summary>
        public byte Unknown22; // 1
        /// <summary>
        /// Unknown long 23.
        /// </summary>
        public long Unknown23;
        /// <summary>
        /// Avatar name is set.
        /// </summary>
        public bool IsNamed;
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
        /// Resource total capacity.
        /// </summary>
        public DataSlotList ResourceTotalCapacity;
        /// <summary>
        /// Resource used capacity.
        /// </summary>
        public DataSlotList ResourceUsedCapacity;
        /// <summary>
        /// Units.
        /// </summary>
        public DataSlotList Units;
        /// <summary>
        /// Spells.
        /// </summary>
        public DataSlotList Spells;
        /// <summary>
        /// Units upgrade level.
        /// </summary>
        public DataSlotList UnitsUpgradeLevel;
        /// <summary>
        /// Spells upgrade level.
        /// </summary>
        public DataSlotList SpellsUpgradeLevel;
        /// <summary>
        /// Heroes upgrade level.
        /// </summary>
        public DataSlotList HeroesUpgradeLevel;
        /// <summary>
        /// Heroes health.
        /// </summary>
        public DataSlotList HeroesHealth;
        /// <summary>
        /// Heroes state.
        /// </summary>
        public DataSlotList HeroesState;
        /// <summary>
        /// AllianceUnits
        /// </summary>
        public UnitDataSlotList AllianceUnits;
        /// <summary>
        /// Tutorial state.
        /// </summary>
        public SingleDataSlotList Tutorial;
        /// <summary>
        /// Acheivement state.
        /// </summary>
        public SingleDataSlotList Acheivement;
        /// <summary>
        /// Acheivement progress.
        /// </summary>
        public DataSlotList AcheivementProgress;
        /// <summary>
        /// Npc stars.
        /// </summary>
        public DataSlotList NpcStars;
        /// <summary>
        /// Npc gold.
        /// </summary>
        public DataSlotList NpcGold;
        /// <summary>
        /// Npc elixir.
        /// </summary>
        public DataSlotList NpcElixir;
        /// <summary>
        /// Unknown integer 28.
        /// </summary>
        public int Unknown28;
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
            //TODO: Implement read.
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

            League = reader.ReadInt32();
            AllianceCastleLevel = reader.ReadInt32();
            AllianceCastleTotalCapacity = reader.ReadInt32();
            AllianceCastleUsedCapacity = reader.ReadInt32();
            TownHallLevel = reader.ReadInt32();
            Name = reader.ReadString();

            Unknown12 = reader.ReadInt32();

            Level = reader.ReadInt32();
            Experience = reader.ReadInt32();
            Gems = reader.ReadInt32();
            FreeGems = reader.ReadInt32();

            Unknown13 = reader.ReadInt32();
            Unknown14 = reader.ReadInt32();

            Trophies = reader.ReadInt32();

            AttacksWon = reader.ReadInt32();
            AttacksLost = reader.ReadInt32();
            DefensesWon = reader.ReadInt32();
            DefensesLost = reader.ReadInt32();
            Unknown19 = reader.ReadInt32();
            Unknown20 = reader.ReadInt32();
            Unknown21 = reader.ReadInt32();
            Unknown22 = reader.ReadByte();
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
            writer.Write(Unknown11);

            writer.Write(League);
            writer.Write(AllianceCastleLevel);
            writer.Write(AllianceCastleTotalCapacity);
            writer.Write(AllianceCastleUsedCapacity);
            writer.Write(TownHallLevel);
            writer.Write(Name);

            writer.Write(Unknown12);

            writer.Write(Level);
            writer.Write(Experience);
            writer.Write(Gems);
            writer.Write(FreeGems);

            writer.Write(Unknown13);
            writer.Write(Unknown14);

            writer.Write(Trophies);

            writer.Write(AttacksWon);
            writer.Write(AttacksLost);
            writer.Write(DefensesWon);
            writer.Write(DefensesLost);

            writer.Write(Unknown19);
            writer.Write(Unknown20);
            writer.Write(Unknown21);

            writer.Write(Unknown22); // if Unknown22 then read?
            writer.Write(Unknown23);

            writer.Write(IsNamed);

            writer.Write(Unknown24);
            writer.Write(Unknown25);
            writer.Write(Unknown26);
            writer.Write(Unknown27);

            ResourceTotalCapacity.Write(writer);
            ResourceUsedCapacity.Write(writer);
            Units.Write(writer);
            Spells.Write(writer);
            UnitsUpgradeLevel.Write(writer);
            SpellsUpgradeLevel.Write(writer);
            HeroesUpgradeLevel.Write(writer);
            HeroesHealth.Write(writer);
            HeroesState.Write(writer);
            Tutorial.Write(writer);
            Acheivement.Write(writer);
            AcheivementProgress.Write(writer);
            NpcStars.Write(writer);
            NpcGold.Write(writer);
            NpcElixir.Write(writer);

            writer.Write(Unknown28);
        }
        #endregion

        //TODO: Make this piece of shit fancier.
#pragma warning disable CS1591
        public class DataSlot
        {
            public DataSlot(int id, int amount)
            {
                ID = id;
                Amount = amount;
            }

            public int ID;
            public int Amount;

            public void Write(MessageWriter writer)
            {
                writer.Write(ID);
                writer.Write(Amount);
            }
        }
        public class DataSlotList
        {
            public DataSlotList()
            {
                List = new List<DataSlot>();
            }

            public List<DataSlot> List;

            public void Write(MessageWriter writer)
            {
                writer.Write(List.Count);
                for (int i = 0; i < List.Count; i++)
                {
                    List[i].Write(writer);
                }
            }
        }

        public class SingleDataSlot
        {
            public SingleDataSlot(int id)
            {
                ID = id;
            }

            public int ID;

            public void Write(MessageWriter writer)
            {
                writer.Write(ID);
            }
        }
        public class SingleDataSlotList
        {
            public SingleDataSlotList()
            {
                List = new List<SingleDataSlot>();
            }

            public List<SingleDataSlot> List;

            public void Write(MessageWriter writer)
            {
                writer.Write(List.Count);
                for (int i = 0; i < List.Count; i++)
                {
                    List[i].Write(writer);
                }
            }
        }

        public class UnitDataSlot
        {
            public UnitDataSlot(int id, int amount)
            {
                ID = id;
                Amount = amount;
            }

            public int ID;
            public int Amount;
            public int Level;

            public void Write(MessageWriter writer)
            {
                writer.Write(ID);
                writer.Write(Amount);
                writer.Write(Level);
            }
        }
        public class UnitDataSlotList
        {
            public UnitDataSlotList()
            {
                List = new List<UnitDataSlot>();
            }

            public List<UnitDataSlot> List;

            public void Write(MessageWriter writer)
            {
                writer.Write(List.Count);
                for (int i = 0; i < List.Count; i++)
                {
                    List[i].Write(writer);
                }
            }
        }
#pragma warning restore CS1591
    }
}
