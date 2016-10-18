using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;

namespace CoCSharp.Server
{
    public partial class AvatarClient
    {
        public void Save()
        {
            if (Status.HasFlag(ClientFlags.Loaded))
                Server.AvatarManager.QueueSave(this);
        }

        public bool Load()
        {
            var avatar = (Avatar)null;
            var newAvatar = false;

            // The client is a new Avatar.
            if (ID == 0 && Token == null)
            {
                avatar = Server.AvatarManager.CreateNewAvatar();
                newAvatar = true;
            }
            else
            {
                // Load from ID & Token. Used for login purposes.
                if (Token != null && ID > 0)
                {
                    if (Server.AvatarManager.Exists(ID))
                    {
                        avatar = Server.AvatarManager.LoadAvatar(ID);

                        // Make sure the tokens corresponds.
                        if (avatar.Token != Token)
                            return false;
                    }
                    else
                    {
                        avatar = Server.AvatarManager.CreateNewAvatar(Token, ID);
                        // If avatar returns null, it means the their is already a entry
                        // with the same ID and we should tell the client to clear its data.

                        newAvatar = true;
                    }
                }
                // Load from ID.
                else if (ID != 0)
                {
                    if (Server.AvatarManager.Exists(ID))
                    {
                        avatar = Server.AvatarManager.LoadAvatar(ID);
                    }
                }
            }

            if (avatar == null)
                return false;

            SetFromAvatar(avatar, newAvatar);
            if (!newAvatar)
            {
                // If the avatar has an alliance associated with it.
                // we load it.
                if (Alliance != null)
                {
                    Alliance = Server.AllianceManager.LoadClan(Alliance.ID);
                }
            }

            Status |= ClientFlags.Loaded;
            return true;
        }

        private void SetNewAvatarSlots()
        {
            var resourcesData = Home.AssetManager.Get<CsvDataTable<ResourceData>>();

            var gold = resourcesData.Rows["Gold"].ID;
            var elixir = resourcesData.Rows["Elixir"].ID;

            NpcStars = Server.NpcManager.CompleteNpcStarList;
            ResourcesAmount.Add(new ResourceAmountSlot(gold, Server.Configuration.StartingGold));
            ResourcesAmount.Add(new ResourceAmountSlot(elixir, Server.Configuration.StartingElixir));
        }

        private void SetFromAvatar(Avatar avatar, bool newAvatar)
        {
            var propValue = IsPropertyChangedEnabled;
            IsPropertyChangedEnabled = false;

            Home = avatar.Home;
            Name = avatar.Name;
            IsNamed = avatar.IsNamed;
            ShieldEndTime = avatar.ShieldEndTime;
            Alliance = avatar.Alliance;
            League = avatar.League;
            ExpLevel = avatar.ExpLevel;
            ExpPoints = avatar.ExpPoints;
            Gems = avatar.Gems;
            FreeGems = avatar.FreeGems;
            Trophies = avatar.Trophies;
            AttacksWon = avatar.AttacksWon;
            AttacksLost = avatar.AttacksLost;
            DefensesWon = avatar.DefensesWon;
            DefensesLost = avatar.DefensesLost;

            ResourcesCapacity = avatar.ResourcesCapacity;
            ResourcesAmount = avatar.ResourcesAmount;
            Units = avatar.Units;
            Spells = avatar.Spells;
            UnitUpgrades = avatar.UnitUpgrades;
            SpellUpgrades = avatar.SpellUpgrades;
            HeroUpgrades = avatar.HeroUpgrades;
            HeroHealths = avatar.HeroHealths;
            HeroStates = avatar.HeroStates;
            AllianceUnits = avatar.AllianceUnits;
            TutorialProgess = avatar.TutorialProgess;
            Acheivements = avatar.Acheivements;
            AcheivementProgress = avatar.AcheivementProgress;
            NpcStars = avatar.NpcStars;
            NpcGold = avatar.NpcGold;
            NpcElixir = avatar.NpcElixir;

            if (newAvatar)
            {
                Token = avatar.Token;
                ID = avatar.ID;

                SetNewAvatarSlots();
            }
        }
    }
}
