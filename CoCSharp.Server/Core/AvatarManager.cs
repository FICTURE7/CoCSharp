using CoCSharp.Logic;
using LiteDB;
using System;
using System.IO;

namespace CoCSharp.Server.Core
{
    // Provides method to save & load avatars.
    public class AvatarManager
    {
        //TODO: Cache starting_village.json

        public AvatarManager()
        {
            _liteDb = new LiteDatabase("avatarsdb.db");

            // Had to downgrade to LiteDB 1.0.4 because
            // we need to prevent the object mapper from mapping the
            // village.
            _liteDb.Mapper.RegisterType
            (
                serialize: (village) => village.ToJson(),
                deserialize: (bson) => Village.FromJson(bson)
            );
            // LiteDB does not support Int64 auto-id. So implement our own.
            _liteDb.Mapper.RegisterAutoId
            (
                isEmpty: (value) => value == 0,
                newId: (collection) => collection.Max("_id").AsInt64 + 1L
            );

            _avatarCollection = _liteDb.GetCollection<Avatar>("avatars");
            _avatarCollection.EnsureIndex("Token", unique: true);
        }

        private LiteDatabase _liteDb;
        private LiteCollection<Avatar> _avatarCollection;

        public Avatar CreateNewAvatar()
        {
            // Generate a token & make sure its unique.
            var token = TokenUtils.GenerateToken();
            while (_avatarCollection.Exists(ava => ava.Token == token))
                token = TokenUtils.GenerateToken();

            var villagePath = Path.Combine(DirectoryPaths.Content, "starting_village.json");
            var villageJson = File.ReadAllText(villagePath);
            var avatar = new Avatar();

            avatar.ShieldEndTime = DateTime.UtcNow.AddDays(3);
            avatar.Token = token;
            avatar.Level = 10; // Skip the tutorials.
            avatar.Home = Village.FromJson(villageJson);
            avatar.Name = "Patrik"; // :]
            avatar.Gems = 300;
            avatar.FreeGems = 300;

            _avatarCollection.Insert(avatar);
            return avatar;
        }

        public Avatar CreateNewAvatar(string token, long id)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;
            if (!TokenUtils.CheckToken(token))
                return null;
            if (Exists(id))
                return null;

            var villagePath = Path.Combine(DirectoryPaths.Content, "starting_village.json");
            var villageJson = File.ReadAllText(villagePath);
            var avatar = new Avatar();

            avatar.ShieldEndTime = DateTime.UtcNow.AddDays(3);
            avatar.Token = token;
            avatar.ID = id;
            avatar.Level = 10; // Skip the tutorials.
            avatar.Home = Village.FromJson(villageJson);
            avatar.Name = "Patrik"; // :]
            avatar.Gems = 300;
            avatar.FreeGems = 300;

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

            return _avatarCollection.FindOne(ava => ava.Token == token);
        }

        public void SaveAvatar(Avatar avatar)
        {
            // If we don't have the avatar in the db
            // we add it to the db.
            if (!_avatarCollection.Update(avatar))
                _avatarCollection.Insert(avatar);
        }
    }
}
