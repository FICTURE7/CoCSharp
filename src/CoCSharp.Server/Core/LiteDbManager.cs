using System;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using LiteDB;

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
