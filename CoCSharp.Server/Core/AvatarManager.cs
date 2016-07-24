using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Server.Handlers.Commands;
using LiteDB;
using System;
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
                deserialize: (bson) => Village.FromJson(bson)
            );

            // Register the serialization of SlotCollections because
            // LiteDB can't seem to do it by himself.
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

            _avatarCollection = _liteDb.GetCollection<Avatar>("avatars");
            // Make sure the Token(UserToken) of avatars are unique.
            _avatarCollection.EnsureIndex("Token", unique: true);
        }

        private string _startingVillage;
        private LiteDatabase _liteDb;
        private LiteCollection<Avatar> _avatarCollection;

        public Avatar CreateNewAvatar()
        {
            // Generate a token & make sure its unique.
            var token = TokenUtils.GenerateToken();
            while (_avatarCollection.Exists(ava => ava.Token == token))
                token = TokenUtils.GenerateToken();

            // CreateNewAvatar will automatically insert the avatar in the db.
            return CreateNewAvatar(token, default(long));
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
            avatar.Level = 9;
            avatar.Home = Village.FromJson(_startingVillage);
            avatar.Name = "Patrik"; // :]
            avatar.Gems = 69696969;
            avatar.FreeGems = 69696969;

            // Set tutorial progress to where it ask the username.
            for (int i = 0; i < 10; i++)
                avatar.TutorialProgess.Add(new TutorialProgressSlot(21000000 + i));

            _avatarCollection.Insert(avatar);
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

        public Avatar LoadAvatar(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var avatar = _avatarCollection.FindOne(ava => ava.Token == token);
            return avatar;
            //var avatar = _avatarCollection.FindOne(ava => ava.Token == token);
            //File.WriteAllText(avatar.Token + ".json", avatar.Home.ToJson());
            //return avatar;
        }

        public void SaveAvatar(Avatar avatar)
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
