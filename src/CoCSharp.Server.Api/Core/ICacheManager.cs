using CoCSharp.Logic;
using CoCSharp.Server.Api.Db;

namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a cache manager that can cache object instances.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Returns a <see cref="LevelSave"/> which was registered to the <see cref="ICacheManager"/> with specified user ID.
        /// </summary>
        /// <param name="userId">User ID which the <see cref="LevelSave"/> was registered with.</param>
        /// <returns><see cref="LevelSave"/> which was registered to the <see cref="ICacheManager"/> with specified user ID.</returns>
        LevelSave GetLevel(long userId);

        /// <summary>
        /// Returns a <see cref="ClanSave"/> which was registered to the <see cref="ICacheManager"/> with specified clan ID. 
        /// </summary>
        /// <param name="clanId">Clan ID which the <see cref="ClanSave"/> was registered with.</param>
        /// <returns><see cref="ClanSave"/> which was registered to the <see cref="ICacheManager"/> with specified clan ID.</returns>
        ClanSave GetClan(long clanId);

        /// <summary>
        /// Adds the specified <see cref="ClanSave"/> to the cache with the specified clan ID as key.
        /// </summary>
        /// <param name="clan"><see cref="ClanSave"/> to store.</param>
        /// <param name="clanId">Clan ID key.</param>
        void RegisterClan(ClanSave clan, long clanId);

        /// <summary>
        /// Adds the specified <see cref="LevelSave"/> to the cache with the specified user ID as key.
        /// </summary>
        /// <param name="level"><see cref="LevelSave"/> to store.</param>
        /// <param name="userId">User ID key.</param>
        void RegisterLevel(LevelSave level, long userId);
    }
}
