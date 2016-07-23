using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Server.Handlers.Commands;
using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Server.Core
{
    // Provides method to save & load avatars.
    public class AvatarManager
    {
        public AvatarManager()
        {
            var villagePath = Path.Combine(DirectoryPaths.Content, "starting_village.json");
            _startingVillage = File.ReadAllText(villagePath);

            _liteDb = new LiteDatabase("avatars_db.db");

            // Had to downgrade to LiteDB 1.0.4 because
            // we need to prevent the object mapper from mapping the
            // village.
            _liteDb.Mapper.RegisterType
            (
                serialize: (village) => village.ToJson(),
                deserialize: (bson) =>
                {
                    var village = Village.FromJson(bson);

                    for (int i = 0; i < village.Buildings.Count; i++)
                        village.Buildings[i].ConstructionFinished += CommandHandlers.BuildingConstructionFinished;

                    for (int i = 0; i < village.Obstacles.Count; i++)
                        village.Obstacles[i].ClearingFinished += CommandHandlers.ObstacleClearingFinished;

                    for (int i = 0; i < village.Traps.Count; i++)
                        village.Traps[i].ConstructionFinished += CommandHandlers.TrapConstructionFinished;

                    return village;
                }
            );

            RegisterSlotCollection<ResourceCapacitySlot>();
            RegisterSlotCollection<ResourceAmountSlot>();
            RegisterSlotCollection<UnitSlot>();
            RegisterSlotCollection<SpellSlot>();
            RegisterSlotCollection<UnitUpgradeSlot>();
            RegisterSlotCollection<SpellUpgradeSlot>();
            RegisterSlotCollection<HeroUpgradeSlot>();
            RegisterSlotCollection<HeroHealthSlot>();
            RegisterSlotCollection<HeroStateSlot>();
            RegisterSlotCollection<AllianceUnitSlot>();
            RegisterSlotCollection<TutorialProgressSlot>();
            RegisterSlotCollection<AchievementSlot>();
            RegisterSlotCollection<AchievementProgessSlot>();
            RegisterSlotCollection<NpcStarSlot>();
            RegisterSlotCollection<NpcGoldSlot>();
            RegisterSlotCollection<NpcElixirSlot>();

            // LiteDB does not support Int64 auto-id. So implement our own.
            _liteDb.Mapper.RegisterAutoId
            (
                isEmpty: (value) => value == default(long),
                newId: (collection) => collection.Max("_id").AsInt64 + 1L
            );

            _avatarCollection = _liteDb.GetCollection<CoCRemoteClient>("avatars");
            _avatarCollection.EnsureIndex("Token", unique: true);
        }

        private string _startingVillage;
        private LiteDatabase _liteDb;
        private LiteCollection<CoCRemoteClient> _avatarCollection;

        public CoCRemoteClient CreateNewAvatar()
        {
            // Generate a token & make sure its unique.
            var token = TokenUtils.GenerateToken();
            while (_avatarCollection.Exists(ava => ava.Token == token))
                token = TokenUtils.GenerateToken();

            var avatar = new Avatar();

            avatar.ShieldEndTime = DateTime.UtcNow.AddDays(3);
            avatar.Token = token;
            // Skip the tutorials.
            avatar.Level = 10;
            avatar.Home = Village.FromJson(_startingVillage);
            avatar.Name = "Patrik"; // :]
            avatar.Gems = 300;
            avatar.FreeGems = 300;

            for (int i = 0; i < 10; i++)
                avatar.TutorialProgess.Add(new TutorialProgressSlot(21000000 + i));

            var client = new CoCRemoteClient();
            client.SetFromAvatar(avatar);

            // Avatar.ID will be automatically incremented by LiteDB.
            _avatarCollection.Insert(client);
            return client;
        }

        public Avatar CreateNewAvatar(string token, long id)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;
            if (!TokenUtils.CheckToken(token))
                return null;
            if (Exists(id))
                return null;

            var avatar = new Avatar();

            avatar.ShieldEndTime = DateTime.UtcNow.AddDays(3);
            avatar.Token = token;
            avatar.ID = id;
            // Skip the tutorials.
            avatar.Level = 9;
            avatar.Home = Village.FromJson(_startingVillage);
            avatar.Name = "Patrik"; // :]
            avatar.Gems = 300;
            avatar.FreeGems = 300;

            for (int i = 0; i < 10; i++)
                avatar.TutorialProgess.Add(new TutorialProgressSlot(21000000 + i));

            var client = new CoCRemoteClient();
            client.SetFromAvatar(avatar);

            _avatarCollection.Insert(client);
            return avatar;
        }

        public bool Exists(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            if (!TokenUtils.CheckToken(token))
                return false;

            return _avatarCollection.Exists(ava => ava.Token == token);
        }

        public bool Exists(long id)
        {
            if (id < 1)
                return false;

            return _avatarCollection.Exists(ava => ava.ID == id);
        }

        public CoCRemoteClient LoadAvatar(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            return _avatarCollection.FindOne(ava => ava.Token == token);

            //var avatar = _avatarCollection.FindOne(ava => ava.Token == token);
            //File.WriteAllText(avatar.Token + ".json", avatar.Home.ToJson());
            //return avatar;
        }

        public void SaveAvatar(CoCRemoteClient avatar)
        {
            // If we don't have the avatar in the db
            // we add it to the db.
            if (!_avatarCollection.Update(avatar))
                _avatarCollection.Insert(avatar);
        }

        public Avatar GetRandomAvatar(long excludeId)
        {
            var count = _avatarCollection.Count();
            var avatars = _avatarCollection.Find(Query.Not("_id", excludeId));
            var choosenOne = (Avatar)null;
            var i = Utils.Random.Next(count);
            var j = 0;
            foreach (var avatar in avatars)
            {
                if (i == j)
                {
                    choosenOne = avatar;
                    break;
                }
                j++;
            }
            return choosenOne;
        }

        private void RegisterSlotCollection<T>() where T : Slot, new()
        {
            _liteDb.Mapper.RegisterType
            (
                serialize: (collection) => _liteDb.Mapper.Serialize(collection.ToArray()),
                deserialize: (bson) => new SlotCollection<T>(_liteDb.Mapper.Deserialize<T[]>(bson))
            );
        }
    }
}
