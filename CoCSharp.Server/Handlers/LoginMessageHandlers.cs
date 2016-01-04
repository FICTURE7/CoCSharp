using CoCSharp.Data;
using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Server.Handlers
{
    public delegate void MessageHandler(CoCServer server, CoCRemoteClient client, Message message);

    public static class LoginMessageHandlers
    {
        private static void HandleLoginRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var encryptionMessage = new EncryptionMessage()
            {
                ServerRandom = CoCCrypto.CreateRandomByteArray(),
                ScramblerVersion = 1
            };

            var lrMessage = message as LoginRequestMessage;
            var lsMessage = new LoginSuccessMessage()
            {
                FacebookID = null,
                GameCenterID = null,
                MajorVersion = 7,
                MinorVersion = 200,
                RevisionVersion = 19,
                ServerEnvironment = "prod",
                LoginCount = 0,
                PlayTime = new TimeSpan(0, 0, 0),
                Unknown1 = 0,
                FacebookAppID = "297484437009394",
                DateLastPlayed = DateTime.Now,
                DateJoined = DateTime.Now,
                Unknown2 = 0,
                GooglePlusID = null,
                CountryCode = "EU"
            };

            if (lrMessage.UserID == 0 && lrMessage.UserToken == null)
            {
                lsMessage.UserID = new Random().Next();
                lsMessage.UserID1 = lsMessage.UserID;
                lsMessage.UserToken = "sup"; //TODO: Implement usertoken gen
            }
            else
            {
                lsMessage.UserID = lrMessage.UserID;
                lsMessage.UserID1 = lrMessage.UserID;
                lsMessage.UserToken = lrMessage.UserToken;
            }

            var avaData = new AvatarData()
            {
                OwnVillageData = new VillageData()
                {
                    Unknown1 = 0,
                    UserID = lsMessage.UserID,
                    ShieldDuration = TimeSpan.FromSeconds(100),
                    Unknown2 = 1200,
                    Unknown3 = 60,
                    Compressed = true,
                    Home = Village.FromJson(File.ReadAllText("ownhome.txt")),
                    Unknown4 = 0
                },
                UserID = lsMessage.UserID,
                UserID2 = lsMessage.UserID,
                OwnClanData = new ClanData()
                {
                    ID = 1,
                    Badge = 0,
                    Name = "CoC#.Server",
                    Level = 5,
                    Role = 1,
                    Unknown1 = 0
                },
                League = 0,
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0,
                TownHallLevel = 3,
                Username = "FICTURE7",

                Unknown12 = -1,

                Experience = 700,
                Level = 14,
                Gems = 999999,
                FreeGems = 999999,

                Unknown13 = 1200,
                Unknown14 = 60,

                Trophies = 600,

                Unknown22 = 1,
                Unknown23 = 946720861000,

                Named = true,

                Unknown24 = 0,
                Unknown25 = 0,
                Unknown26 = 0,
                Unknown27 = 1,

                ResourceTotalCapacity = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                ResourceUsedCapacity = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                Units = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                Spells = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                UnitsUpgradeLevel = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                SpellsUpgradeLevel = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                HeroesUpgradeLevel = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                HeroesHealth = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                HeroesState = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                Tutorial = new AvatarData.SingleDataSlotList()
                {
                    List = new List<AvatarData.SingleDataSlot>()
                },

                Acheivement = new AvatarData.SingleDataSlotList()
                {
                    List = new List<AvatarData.SingleDataSlot>()
                },

                AcheivementProgress = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                NpcStars = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                NpcGold = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                NpcElixir = new AvatarData.DataSlotList()
                {
                    List = new List<AvatarData.DataSlot>()
                },

                Unknown28 = 0
            };

            var ohdMessage = new OwnHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(100),
                Unknown1 = -1,
                Timestamp = DateTime.Now,
                OwnAvatarData = avaData
            };

            client.NetworkManager.SendMessage(encryptionMessage);
            client.NetworkManager.SendMessage(lsMessage);
            client.NetworkManager.SendMessage(ohdMessage);
        }

        public static void RegisterLoginMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new LoginRequestMessage(), HandleLoginRequestMessage);
        }
    }
}
