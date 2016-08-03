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
            _saveQueue = new SaveQueue<Clan>();
            _deleteQueue = new SaveQueue<Clan>();

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

        // Queue of alliances to save.
        private readonly SaveQueue<Clan> _saveQueue;
        // Queue of alliances to delete.
        private readonly SaveQueue<Clan> _deleteQueue;

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

            return _alliancesCollection.FindById(id);
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

        public void DeleteClan(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            _alliancesCollection.Delete(clan.ID);
        }

        public void QueueSave(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            _saveQueue.Enqueue(clan);
        }

        public void QueueDelete(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            _deleteQueue.Enqueue(clan);
        }

        public void Flush()
        {
            if (_saveQueue.Count == 0)
                return;

            Debug.WriteLine("Flushing alliances", "Saving");
            Debug.Indent();

            // Save transaction.
            using (var trans = _liteDb.BeginTrans())
            {
                for (int i = 0; i < _saveQueue.Count; i++)
                {
                    var clan = _saveQueue.Dequeue();
                    Debug.Assert(clan != null);
                    if (clan == null)
                    {
                        Console.WriteLine("WARNING: AllianceManger _saveQueue.Dequeue() returned null - skipping save");
                        continue;
                    }

                    SaveClan(clan);
                    Debug.WriteLine("--> Saved alliance " + clan.ID, "Saving");
                }
            }

            // Delete transaction.
            using (var trans = _liteDb.BeginTrans())
            {
                for (int i = 0; i < _deleteQueue.Count; i++)
                {
                    var clan = _saveQueue.Dequeue();
                    Debug.Assert(clan != null);
                    if (clan == null)
                    {
                        Console.WriteLine("alliance _deleteQueue.Dequeue() returned null - skipping save");
                        continue;
                    }

                    DeleteClan(clan);
                    Debug.WriteLine("--> Deleted alliance " + clan.ID, "Saving");
                }
            }
            Debug.Unindent();
            Debug.WriteLine("Flushing done", "Saving");
        }

        private void OnLog(string log)
        {
            Console.WriteLine("ERROR: AllianceManager _liteDb: " + log);
        }

        public IEnumerable<Clan> GetAllClan()
        {
            return _alliancesCollection.FindAll();
        }
    }
}
