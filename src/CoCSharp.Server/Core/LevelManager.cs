using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Core;
using System;
using CoCSharp.Logic;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace CoCSharp.Server.Core
{
    public class LevelManager : ILevelManager
    {
        #region Constructors
        public LevelManager(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _random = new Random();
            _loaded = new List<Level>();
        }
        #endregion

        #region Fields & Properties
        private readonly Random _random;
        private readonly IServer _server;
        private readonly List<Level> _loaded;

        public IServer Server => _server;

        public IReadOnlyCollection<Level> Loaded => _loaded.AsReadOnly();
        #endregion

        #region Methods
        public async Task<Level> GetLevelAsync(long userId, CancellationToken cancellationToken)
        {
            if (_loaded.Count > 0)
            {
                for (int i = 0; i < _loaded.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tmp = _loaded[i];
                    if (tmp.Avatar.Id == userId)
                        return tmp;
                }
            }

            var save = await Server.Db.LoadLevelAsync(userId, cancellationToken);
            if (save == null)
                return null;

            var level = save.ToLevel(Server.Assets);
            if (save.ClanId != null)
            {
                var clanId = save.ClanId.Value;
                var clan = await Server.Clans.GetClanAsync(clanId, cancellationToken);

                level.Avatar.Alliance = clan;
            }

            lock (_loaded)
            {
                _loaded.Add(level);
            }

            return level;
        }

        public async Task<Level> NewLevelAsync(CancellationToken cancellationToken)
        {
            var save = await Server.Db.NewLevelAsync(cancellationToken).ConfigureAwait(false);
            var level = save.ToLevel(Server.Assets);
            lock (_loaded)
            {
                _loaded.Add(level);
            }

            return level;
        }

        public async Task<Level> NewLevelAsync(long userId, string userToken, CancellationToken cancellationToken)
        {
            var save = await Server.Db.NewLevelAsync(userId, userToken, cancellationToken).ConfigureAwait(false);
            var level = save.ToLevel(Server.Assets);
            lock (_loaded)
            {
                _loaded.Add(level);
            }

            return level;
        }

        public async Task<Level> GetRandomLevelAsync(CancellationToken cancellationToken)
        {
            var fromLoaded = _random.Next(1) > 0;
            if (_loaded.Count > 0 && fromLoaded)
            {
                var index = _random.Next(_loaded.Count - 1);
                return _loaded[index];
            }

            var save = await Server.Db.RandomLevelAsync(cancellationToken);
            var level = save.ToLevel(Server.Assets);

            lock (_loaded)
            {
                _loaded.Add(level);
            }

            return level;
        }
        #endregion
    }
}
