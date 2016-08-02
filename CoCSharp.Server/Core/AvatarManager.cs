using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using LiteDB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CoCSharp.Server.Core
{
    // Provides method to save & load avatars.
    public class AvatarManager
    {
        public AvatarManager()
        {
            _lock = new object();
            _saveQueue = new List<Avatar>(32);

            // Random names which Avatars who have not set their names
            // are set to.
            _newNames = new string[]
            {
                "Patrik", // :]
                "Kevin",
                "John",
                "Kukli",
                "Jessen",
                "Levi",
                "Kenny"
            };

            var villagePath = Path.Combine(DirectoryPaths.Content, "starting_village.json");
            _startingVillage = File.ReadAllText(villagePath);

            _mapper = new BsonMapper();
            // LiteDB does not support Int64 auto-id. So implement our own.
            _mapper.RegisterAutoId
            (
                isEmpty: (value) =>
                {
                    return value == default(long);
                },
                newId: (collection) =>
                {
                    return collection.Max("_id").AsInt64 + 1L;
                }
            );

            // Make sure LiteDB does not map Villages into BSON.
            _mapper.RegisterType
            (
                serialize: (village) =>
                {
                    return village.ToJson();
                },
                deserialize: (bson) =>
                {
                    return Village.FromJson(bson);
                }
            );

            // Make sure LiteDB does not serialize Clans.
            // Instead serialize their IDs.
            _mapper.RegisterType
            (
                serialize: (clan) =>
                {
                    return clan.ID;
                },
                deserialize: (bson) =>
                {
                    return new Clan()
                    {
                        ID = bson.AsInt64
                    };
                }
            );

            // LiteDB does not take into account inheritance, from what I observed.
            _mapper.Entity<Avatar>()
                   .Id(x => x.ID)
                   .Index(x => x.Token, true) // Make sure tokens are unique.
                   .Ignore(x => x.OwnHomeDataMessage)
                   .Ignore(x => x.ShieldDuration)
                   .Ignore(x => x.IsPropertyChangedEnabled);
            _mapper.Entity<AvatarClient>()
                   .Id(x => x.ID)
                   .Index(x => x.Token, true) // Make sure tokens are unique.
                   .Ignore(x => x.OwnHomeDataMessage)
                   .Ignore(x => x.ShieldDuration)
                   .Ignore(x => x.IsPropertyChangedEnabled);


            _liteDb = new LiteDatabase("avatars_db.db", _mapper);
            _avatarCollection = _liteDb.GetCollection<Avatar>("avatars");
        }

        // Array of random names to choose from when creating a new avatar.
        private readonly string[] _newNames;
        // Default village json.
        private readonly string _startingVillage;

        // Mapper that _liteDb is going to use.
        private readonly BsonMapper _mapper;
        // Db of Avatars.
        private readonly LiteDatabase _liteDb;
        // Collection of avatars in _liteDb.
        private readonly LiteCollection<Avatar> _avatarCollection;
        // 'Queue' of avatars to save.
        private readonly List<Avatar> _saveQueue;
        // To lock reading and writing to _saveQueue.
        private readonly object _lock;

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
            avatar.Name = GetRandomName();
            avatar.Gems = 69696969;
            avatar.FreeGems = 69696969;

            // Set tutorial progress to where it ask the username.
            for (int i = 0; i < 10; i++)
                avatar.TutorialProgess.Add(new TutorialProgressSlot(21000000 + i));

            SaveAvatar(avatar);
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

            //var avatar = _avatarCollection.FindOne(ava => ava.Token == token);
            //File.WriteAllText(avatar.Token + ".json", avatar.Home.ToJson());
            //return avatar;
        }

        public Avatar LoadAvatar(long id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException("id", "id cannot be less than 1.");

            return _avatarCollection.FindById(id);
        }

        public void SaveAvatar(Avatar avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            // If we don't have the avatar in the db
            // we add it to the db.
            if (!_avatarCollection.Update(avatar))
                _avatarCollection.Insert(avatar);
        }

        // Queues the specified avatar into a list
        // of avatars to save.
        public void Queue(Avatar avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            lock (_lock)
            {
                var index = _saveQueue.IndexOf(avatar);
                if (index == -1)
                {
                    _saveQueue.Add(avatar);
                }
                else
                {
                    // If we already queued this avatar
                    // move it to the end of the queue.
                    _saveQueue.RemoveAt(index);
                    _saveQueue.Add(avatar);
                }
            }
        }

        // Flushes all avatars in the save queue.
        public void Flush()
        {
            lock (_lock)
            {
                if (_saveQueue.Count == 0)
                    return;

                Debug.WriteLine("Flushing avatars", "Saving");
                using (var trans = _liteDb.BeginTrans())
                {
                    for (int i = 0; i < _saveQueue.Count; i++)
                    {
                        var avatar = _saveQueue[i];
                        _saveQueue.RemoveAt(i);

                        Debug.Assert(avatar != null);
                        SaveAvatar(avatar);
                        Debug.WriteLine("--> Saved avatar " + avatar.Token, "Saving");
                    }

                    trans.Commit();
                }
                Debug.WriteLine("Flushing done", "Saving");
            }
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

        // Returns a random name from _newNames.
        private string GetRandomName()
        {
            return _newNames[Utils.Random.Next(_newNames.Length - 1)];
        }
    }
}
