using CoCSharp.Logic;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a manager that manages <see cref="Level"/> instances.
    /// </summary>
    public interface ILevelManager
    {
        IReadOnlyCollection<Level> Loaded { get; }

        Task<Level> GetLevelAsync(long userId, CancellationToken cancellationToken);

        Task<Level> NewLevelAsync(CancellationToken cancellationToken);

        Task<Level> NewLevelAsync(long userId, string userToken, CancellationToken cancellationToken);

        Task<Level> GetRandomLevelAsync(CancellationToken cancellationToken);
    }
}
