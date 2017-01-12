using CoCSharp.Logic;
using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Core
{
    public class ClanManager : IClanManager
    {
        public ClanManager(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _loaded = new List<Clan>();
        }

        private readonly IServer _server;
        private readonly List<Clan> _loaded;

        public IServer Server => _server;

        public IReadOnlyCollection<Clan> Loaded => _loaded.AsReadOnly();

        public async Task<Clan> GetClanAsync(long clanId, CancellationToken cancellationToken)
        {
            if (_loaded.Count > 0)
            {
                for (int i = 0; i < _loaded.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tmp = _loaded[i];
                    if (tmp.Id == clanId)
                        return tmp;
                }
            }

            var save = await Server.Db.LoadClanAsync(clanId, cancellationToken);
            var clan = save.ToClan();

            lock (_loaded)
            {
                _loaded.Add(clan);
            }

            return clan;
        }
    }
}
