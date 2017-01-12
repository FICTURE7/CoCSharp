using CoCSharp.Logic;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a manager that manages <see cref="Clan"/> instances.
    /// </summary>
    public interface IClanManager
    {
        IReadOnlyCollection<Clan> Loaded { get; }

        Task<Clan> GetClanAsync(long clanId, CancellationToken cancellationToken);
    }
}
