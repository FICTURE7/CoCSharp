using CoCSharp.Logic;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Server.Core
{
    public class AllianceManager
    {
        public AllianceManager()
        {
            _lock = new object();
            _saveQueue = new List<Clan>(32);

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

            _mapper.Entity<Clan>()
                   .Ignore(x => x.AllianceDataResponseMessage);

            _liteDb = new LiteDatabase("alliances_db.db", _mapper);
            _liteDb.Log.Level = Logger.ERROR;
            _liteDb.Log.Logging += OnLog;

            _alliancesCollection = _liteDb.GetCollection<Clan>("alliances");
        }

        // Mapper that _liteDb is going to use.
        private readonly BsonMapper _mapper;
        // Db of alliances.
        private readonly LiteDatabase _liteDb;
        // Collection of alliances in _liteDb.
        private readonly LiteCollection<Clan> _alliancesCollection;
        // 'Queue' of alliances to save.
        private readonly List<Clan> _saveQueue;
        // To prevent reading and writing to _saveQueue.
        private readonly object _lock;

        public Clan CreateNewClan()
        {
            var clan = new Clan();

            _alliancesCollection.Insert(clan);
            return clan;
        }

        public Clan LoadClan(long id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException("id", "id cannot be less than 1.");

            return _alliancesCollection.FindOne(c => c.ID == id);
        }

        public void SaveClan(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            using (var trans = _liteDb.BeginTrans())
            {
                if (!_alliancesCollection.Update(clan))
                    _alliancesCollection.Insert(clan);
            }
        }

        public void Delete(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            _alliancesCollection.Delete(clan.ID);
        }

        public void Queue(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            lock (_lock)
            {
                var index = _saveQueue.IndexOf(clan);
                if (index == -1)
                {
                    _saveQueue.Add(clan);
                }
                else
                {
                    // If we already queued this clan
                    // move it to the end of the queue.
                    _saveQueue.RemoveAt(index);
                    _saveQueue.Add(clan);
                }
            }
        }

        public void Flush()
        {
            lock (_lock)
            {
                if (_saveQueue.Count == 0)
                    return;

                using (var trans = _liteDb.BeginTrans())
                {
                    for (int i = 0; i < _saveQueue.Count; i++)
                    {
                        var clan = _saveQueue[i];
                        _saveQueue.RemoveAt(i);

                        Debug.Assert(clan != null);
                        SaveClan(clan);
                        Debug.WriteLine("--> Saved alliance " + clan.ID, "Saving");
                    }

                    // trans.Commit();
                }
            }
        }

        private void OnLog(string obj)
        {
            Console.WriteLine("AllianceDb exception: " + obj);
        }

        public IEnumerable<Clan> GetAllClan()
        {
            return _alliancesCollection.FindAll();
        }
    }
}
