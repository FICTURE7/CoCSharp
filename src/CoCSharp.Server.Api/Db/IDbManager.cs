using CoCSharp.Logic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Api.Db
{
    /// <summary>
    /// Interface representing a database manager that can load and save clans and levels from a database.
    /// </summary>
    public interface IDbManager : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IServer"/> which owns the <see cref="IDbManager"/>.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Loads a <see cref="Level"/> with the specified ID asynchronously.
        /// </summary>
        /// <param name="userId">ID of <see cref="Level"/> to load.</param>
        /// <returns>Returns the <see cref="Level"/> that was loaded if found; otherwise <c>null</c>.</returns>
        Task<LevelSave> LoadLevelAsync(long userId, CancellationToken token);

        /// <summary>
        /// Saves the specified <see cref="LevelSave"/> to the database asynchronously.
        /// </summary>
        /// <param name="level"><see cref="LevelSave"/> to save.</param>
        Task SaveLevelAsync(LevelSave level, CancellationToken token);

        /// <summary>
        /// Returns a new <see cref="LevelSave"/>.
        /// </summary>
        /// <returns>A new <see cref="LevelSave"/>.</returns>
        Task<LevelSave> NewLevelAsync(CancellationToken token);

        /// <summary>
        /// Returns a new <see cref="LevelSave"/> with the specified token and ID.
        /// </summary>
        /// <param name="userId">ID of the new <see cref="LevelSave"/>.</param>
        /// <param name="userToken">Token of the new <see cref="LevelSave"/>.</param>
        /// <returns>A new <see cref="LevelSave"/> with the specified token and ID.</returns>
        Task<LevelSave> NewLevelAsync(long userId, string userToken, CancellationToken token);

        /// <summary>
        /// Returns a random <see cref="LevelSave"/> from the database asynchronously.
        /// </summary>
        /// <returns>A random <see cref="LevelSave"/> from the database.</returns>
        Task<LevelSave> RandomLevelAsync(CancellationToken token);

        /// <summary>
        /// Loads a <see cref="ClanSave"/> with the specified ID asynchronously.
        /// </summary>
        /// <param name="clanId">ID of <see cref="ClanSave"/> to load.</param>
        /// <returns>Returns the <see cref="ClanSave"/> that was loaded if found; otherwise <c>null</c>.</returns>
        Task<ClanSave> LoadClanAsync(long clanId, CancellationToken token);

        /// <summary>
        /// Saves the specified <see cref="ClanSave"/> to the database asynchronously.
        /// </summary>
        /// <param name="clan"><see cref="ClanSave"/> to save.</param>
        Task SaveClanAsync(ClanSave clan, CancellationToken token);

        /// <summary>
        /// Returns a new <see cref="ClanSave"/> asynchronously.
        /// </summary>
        /// <returns>A new <see cref="ClanSave"/>.</returns>
        Task<ClanSave> NewClanAsync(CancellationToken token);

        /// <summary>
        /// Searches for clans for the specified <see cref="Level"/> with the specified <see cref="ClanQuery"/> search.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<IEnumerable<ClanSave>> SearchClansAsync(Level level, ClanQuery search, CancellationToken token);

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> that iterates through all the clans that the specified level can join.
        /// </summary>
        /// <param name="level"><see cref="Level"/> that join-able clans will be searched for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that iterates through all the clans.</returns>
        Task<IEnumerable<ClanSave>> SearchClansAsync(Level level, CancellationToken token);

        /// <summary>
        /// Returns the number of <see cref="LevelSave"/> in the database.
        /// </summary>
        /// <returns>Number of <see cref="LevelSave"/> in the database.</returns>
        Task<long> GetLevelCountAsync();

        /// <summary>
        /// Returns the number of <see cref="ClanSave"/> in the database.
        /// </summary>
        /// <returns>Number of <see cref="ClanSave"/> in the database.</returns>
        Task<long> GetClanCountAsync();
    }
}
