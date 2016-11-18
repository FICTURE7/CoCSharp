using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using LiteDB;
using System;
using System.Linq;

namespace CoCSharp.Server.Core
{
    public class LiteDbManager : IDbManager
    {
        #region Constructors
        public LiteDbManager(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            var mapper = new BsonMapper();
            mapper.Entity<LevelSave>();
            mapper.RegisterAutoId
            (
                isEmpty: (value) => value == default(long),
                newId: (collection) => collection.Max("_id").AsInt64 + 1L
            );

            _server = server;

            var resourceTable = _server.Assets.Get<CsvDataTable<ResourceData>>();
            _goldId = resourceTable.Rows["Gold"].ID;
            _elixirId = resourceTable.Rows["Elixir"].ID;
            _gemsId = resourceTable.Rows["Diamonds"].ID;

            _startingGold = _server.Configuration.StartingGold;
            _startingElixir = _server.Configuration.StartingElixir;
            _startingGems = _server.Configuration.StartingGems;
            _startingVillage = _server.Configuration.StartingVillage;

            _db = new LiteDatabase("coc_litedb.db", mapper);
            _levels = _db.GetCollection<LevelSave>("levels");
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;

        private readonly int _goldId;
        private readonly int _elixirId;
        private readonly int _gemsId;

        private readonly int _startingGold;
        private readonly int _startingElixir;
        private readonly int _startingGems;
        private readonly string _startingVillage;

        private readonly IServer _server;
        private readonly LiteDatabase _db;
        private readonly LiteCollection<LevelSave> _levels;

        public IServer Server => _server;
        public int TotalEntries => _levels.Count();
        #endregion

        #region Methods
        public ILevelSave LoadLevel(long id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id), "ID of level must be greater or equal to 1.");

            return _levels.FindById(id);
        }

        private Random _random = new Random();
        public ILevelSave RandomLevel()
        {
            using (var trans = _db.BeginTrans())
            {
                var totalEntries = TotalEntries;
                var skipCount = _random.Next(totalEntries);
                var lvl = _levels.FindAll().Skip(skipCount).FirstOrDefault();
                return lvl;
            }
        }

        public void SaveLevel(ILevelSave level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            var nlevel = (LevelSave)level;
            using (var trans = _db.BeginTrans())
            {
                if (!_levels.Update(nlevel))
                    _levels.Insert(nlevel);
            }

            nlevel.LastSave = DateTime.UtcNow;
        }

        public ILevelSave NewLevel()
        {
            var token = TokenUtils.GenerateToken();
            return InternalNewLevel(0, token);
        }

        public ILevelSave NewLevel(long id, string token)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException();
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return InternalNewLevel(id, token);
        }

        private ILevelSave InternalNewLevel(long id, string token)
        {
            var save = new LevelSave
            {
                ID = id,
                Token = token,

                // Prevent client from crashing.
                // Because a name == null, will cause the client to crash.
                Name = GetRandomName(),

                Gems = _startingGems,
                FreeGems = _startingGems,

                // Prevent client from crashing.
                // Because a level less than 1 will cause the client to crash.
                ExpLevel = 1,

                VillageJson = _startingVillage,
            };

            var tutorialProgress = new TutorialProgressSlot[10];
            for (int i = 0; i < 10; i++)
                tutorialProgress[i] = new TutorialProgressSlot(21000000 + i);

            var resourceAmount = new ResourceAmountSlot[]
            {
                new ResourceAmountSlot(_goldId, _startingGold),
                new ResourceAmountSlot(_elixirId, _startingElixir),
                new ResourceAmountSlot(_gemsId, _startingGems)
            };

            save.TutorialProgress = tutorialProgress;
            save.ResourcesAmount = resourceAmount;

            // Save LevelSave to set the ID using the AutoId stuff.
            SaveLevel(save);
            return save;
        }

        private static readonly Random s_random = new Random();
        private static readonly string[] s_names =
        {
            "Patrik",
            "Jean",
            "Kenny"
        };
        private static readonly int s_nameCount = s_names.Length;
        private static string GetRandomName()
        {
            return s_names[s_random.Next(s_nameCount)];
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _db.Dispose();
            _disposed = true;
        }
        #endregion
    }
}
