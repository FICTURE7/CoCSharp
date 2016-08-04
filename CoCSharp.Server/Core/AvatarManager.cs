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
        public AvatarManager(CoCServer server)
        {
            _server = server;
            _saveQueue = new SaveQueue<Avatar>();

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
                   //.Index(x => x.Token, true) // Make sure tokens are unique.
                   .Ignore(x => x.OwnHomeDataMessage)
                   .Ignore(x => x.ShieldDuration)
                   .Ignore(x => x.IsPropertyChangedEnabled);

            _mapper.Entity<AvatarClient>()
                   .Id(x => x.ID)
                   //.Index(x => x.Token, true) // Make sure tokens are unique.
                   .Ignore(x => x.OwnHomeDataMessage)
                   .Ignore(x => x.ShieldDuration)
                   .Ignore(x => x.IsPropertyChangedEnabled);


            _dbLock = new object();
            _liteDb = new LiteDatabase("avatars_db.db", _mapper);
            _liteDb.Log.Level = Logger.ERROR;
            _liteDb.Log.Logging += OnLog;

            _avatarCollection = _liteDb.GetCollection<Avatar>("avatars");
        }

        // Server with which the manager was initialized.
        private readonly CoCServer _server;
        // Array of random names to choose from when creating a new avatar.
        private readonly string[] _newNames;
        // Default village json.
        private readonly string _startingVillage;

        // Mapper that _liteDb is going to use.
        private readonly BsonMapper _mapper;

        private readonly object _dbLock;
        // Db of Avatars.
        private readonly LiteDatabase _liteDb;
        // Collection of avatars in _liteDb.
        private readonly LiteCollection<Avatar> _avatarCollection;

        // Queue of avatars to save.
        private readonly SaveQueue<Avatar> _saveQueue;

        public Avatar CreateNewAvatar()
        {
            var token = TokenUtils.GenerateToken();

            // CreateNewAvatar will automatically insert the avatar in the db.
            return CreateNewAvatar(token, default(long));
        }

        public Avatar CreateNewAvatar(string token, long id)
        {
            if (Exists(id))
                return null;

            var avatar = new Avatar();

            avatar.ShieldEndTime = DateTime.UtcNow.AddDays(3);
            avatar.Token = token;
            avatar.ID = id;
            avatar.Level = 9;
            avatar.Home = Village.FromJson(_startingVillage);
            avatar.Name = GetRandomName();
            avatar.Gems = _server.Configuration.StartingGems;
            avatar.FreeGems = _server.Configuration.StartingGems;

            // Set tutorial progress to where it ask the username.
            for (int i = 0; i < 10; i++)
                avatar.TutorialProgess.Add(new TutorialProgressSlot(21000000 + i));

            SaveAvatar(avatar);
            return avatar;
        }

        public bool Exists(string token)
        {
            lock (_dbLock)
            {
                return _avatarCollection.Exists(ava => ava.Token == token);
            }
        }

        public bool Exists(long id)
        {
            if (id < 1)
                return false;

            lock (_dbLock)
            {
                return _avatarCollection.Exists(ava => ava.ID == id);
            }
        }

        public Avatar LoadAvatar(string token)
        {
            Debug.Assert(token != null);

            lock (_dbLock)
            {
                return _avatarCollection.FindOne(ava => ava.Token == token);
            }
        }

        public Avatar LoadAvatar(long id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException("id", "id cannot be less than 1.");

            lock (_dbLock)
            {
                return _avatarCollection.FindById(id);
            }
        }

        public void SaveAvatar(Avatar avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            Debug.Assert(avatar.Token != null);
            if (avatar.Token == null)
            {
                Console.WriteLine("warning: null token");
                return;
            }

            lock (_dbLock)
            {
                var trans = _liteDb.BeginTrans();

                // If we don't have the avatar in the db
                // we add it to the db.
                if (!_avatarCollection.Update(avatar))
                    _avatarCollection.Insert(avatar);

                trans.Commit();
            }
        }

        // Queues the specified avatar into a list
        // of avatars to save.
        public void QueueSave(Avatar avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            _saveQueue.Enqueue(avatar);
        }

        // Flushes all avatars in the save queue.
        public void Flush()
        {
            if (_saveQueue.Count == 0)
                return;

            Debug.WriteLine("Flushing avatars", "Saving");
            Debug.Indent();

            lock (_dbLock)
            {
                // Save transaction.
                var trans = _liteDb.BeginTrans();
                for (int i = 0; i < _saveQueue.Count; i++)
                {
                    var avatar = _saveQueue.Dequeue();
                    Debug.Assert(avatar != null);
                    if (avatar == null)
                    {
                        Console.WriteLine("WARNING: AllianceManager _saveQueue.Dequeue() returned null - skipping save");
                        continue;
                    }

                    SaveAvatar(avatar);
                    Debug.WriteLine("--> Saved avatar " + avatar.Token, "Saving");
                }
                trans.Commit();
            }

            Debug.Unindent();
            Debug.WriteLine("Flushing done", "Saving");
        }

        private void OnLog(string log)
        {
            Console.WriteLine("ERROR: AvatarManager _liteDb: " + log);
        }

        public Avatar GetRandomAvatar(long excludeId)
        {
            var count = 0;
            var avatars = (IEnumerable<Avatar>)null;
            lock (_dbLock)
            {
                count = _avatarCollection.Count();
                avatars = _avatarCollection.Find(Query.Not("_id", excludeId));
            }

            var choosenOne = (Avatar)null;
            var i = Utils.Random.Next(count);
            var j = 0;

            Debug.Assert(avatars != null);
            if (avatars == null)
            {
                Console.WriteLine("warning: db returned null list of avatars");
                return null;
            }

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
