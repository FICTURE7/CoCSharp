using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Server.Core;
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
        public CoCServer Server { get; private set; }
        [BsonIgnore]
        public Socket Connection { get; private set; }
        [BsonIgnore]
        public NetworkManagerAsync NetworkManager { get; private set; }
        [BsonIgnore]
        public byte[] SessionKey { get; set; }

        //[BsonField]
        //internal ResourceAmountSlot[] SaveResourcesAmount
        //{
        //    get
        //    {
        //        return ResourcesAmount.ToArray();
        //    }
        //    set
        //    {
        //        ResourcesAmount.Clear();
        //        for (int i = 0; i < value.Length; i++)
        //            ResourcesAmount.Add(value[i]);
        //    }
        //}

        //[BsonField]
        //internal UnitSlot[] SaveUnits
        //{
        //    get
        //    {
        //        return Units.ToArray();
        //    }
        //    set
        //    {
        //        Units.Clear();
        //        for (int i = 0; i < value.Length; i++)
        //            Units.Add(value[i]);
        //    }
        //}

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

        public void SetFromAvatar(Avatar avatar)
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
