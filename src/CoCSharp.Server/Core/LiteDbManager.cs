using CoCSharp.Data.Slots;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using LiteDB;
using System;

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
            _db = new LiteDatabase("coc_litedb.db", mapper);
            _levels = _db.GetCollection<LevelSave>("levels");
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;
        private readonly IServer _server;
        private readonly LiteDatabase _db;
        private readonly LiteCollection<LevelSave> _levels;

        public IServer Server => _server;
        #endregion

        #region Methods
        public ILevelSave LoadLevel(long id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id), "ID of level must be greater or equal to 1.");

            return _levels.FindById(id);
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

                Name = GetRandomName(),

                Gems = Server.Configuration.StartingGems,
                FreeGems = Server.Configuration.StartingGems,
                VillageJson = Server.Configuration.StartingVillage
            };

            var tutorialProgress = new TutorialProgressSlot[10];
            for (int i = 0; i < 10; i++)
                tutorialProgress[i] = new TutorialProgressSlot(21000000 + i);

            save.TutorialProgress = tutorialProgress;
             
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
