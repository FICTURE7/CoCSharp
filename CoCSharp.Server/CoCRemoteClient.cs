using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Server.Core;
using CoCSharp.Server.Handlers.Commands;
using LiteDB;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace CoCSharp.Server
{
    public class CoCRemoteClient : Avatar
    {
        public CoCRemoteClient() : base()
        {
            // Space
        }

        public CoCRemoteClient(CoCServer server, Socket connection, NetworkManagerAsyncSettings settings) : base()
        {
            Server = server;

            Connection = connection;
            SessionKey = null;
            NetworkManager = new NetworkManagerAsync(connection, settings);
            NetworkManager.MessageReceived += OnMessageReceived;
            NetworkManager.Disconnected += OnDisconnected;
        }

        [BsonIgnore]
        public CoCServer Server { get; internal set; }
        [BsonIgnore]
        public Socket Connection { get; private set; }
        [BsonIgnore]
        public NetworkManagerAsync NetworkManager { get; private set; }
        [BsonIgnore]
        public byte[] SessionKey { get; set; }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Exception != null)
            {
                Console.WriteLine("Exception occurred while receiving: {0}", e.Exception.ToString());
            }
            if (e.Exception is CryptographicException)
            {
                Console.WriteLine("\tCryptographicException occurred while decrypting a message.");
                //TODO: Disconnect the client.
                return;
            }

            Server.HandleMessage(this, e.Message);
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            // Push the VillageObjects to the VillageObjectPool.
            if (Home != null)
            {
                Home.Dispose();
            }

            // Dereference the client object so that it gets picked up
            // by the GarbageCollector.
            Server.Clients.Remove(this);
            FancyConsole.WriteLine("[&(darkyellow)Listener&(default)] -> Avatar &(darkcyan){0}&(default) disconnected.", Token);
        }

        public void Save()
        {
            Server.AvatarManager.SaveAvatar(this);
        }

        public bool Load()
        {
            var avatar = (Avatar)null;

            // The client is a new Avatar.
            if (ID == 0 && Token == null)
            {
                avatar = Server.AvatarManager.CreateNewAvatar();
                avatar.ResourcesAmount.Add(new ResourceAmountSlot(3000001, 1000));
                avatar.ResourcesAmount.Add(new ResourceAmountSlot(3000000, 1000));
            }
            else
            {
                if (Server.AvatarManager.Exists(Token))
                {
                    avatar = Server.AvatarManager.LoadAvatar(Token);
                }
                else
                {
                    avatar = Server.AvatarManager.CreateNewAvatar(Token, ID);
                }
            }

            if (avatar == null)
                return false;

            // Set the UserToken of buildings to this instance.
            for (int i = 0; i < avatar.Home.Buildings.Count; i++)
            {
                var building = avatar.Home.Buildings[i];
                building.UserToken = this;
                building.ConstructionFinished += CommandHandlers.BuildingConstructionFinished;
            }

            for (int i = 0; i < avatar.Home.Traps.Count; i++)
            {
                var trap = avatar.Home.Traps[i];
                trap.UserToken = this;
                trap.ConstructionFinished += CommandHandlers.TrapConstructionFinished;
            }

            for (int i = 0; i < avatar.Home.Obstacles.Count; i++)
            {
                var obstacle = avatar.Home.Obstacles[i];
                obstacle.UserToken = this;
                obstacle.ClearingFinished += CommandHandlers.ObstacleClearingFinished;
            }

            SetFromAvatar(avatar);
            return true;
        }

        private void SetFromAvatar(Avatar avatar)
        {
            var propValue = IsPropertyChangedEnabled;
            IsPropertyChangedEnabled = false;

            Name = avatar.Name;
            IsNamed = avatar.IsNamed;
            Token = avatar.Token;
            ID = avatar.ID;
            ShieldEndTime = avatar.ShieldEndTime;
            Home = avatar.Home;
            Alliance = avatar.Alliance;
            League = avatar.League;
            Level = avatar.Level;
            Experience = avatar.Experience;
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
        }
    }
}
