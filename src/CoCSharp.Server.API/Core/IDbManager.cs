using CoCSharp.Logic;
using System;

namespace CoCSharp.Server.API.Core
{
    /// <summary>
    /// Represents a database manager.
    /// </summary>
    public interface IDbManager : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IServer"/> which owns the <see cref="IDbManager"/>.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Loads a <see cref="Level"/> with the specified ID.
        /// </summary>
        /// <param name="id">ID of <see cref="Level"/> to load.</param>
        /// <returns>Returns the <see cref="Level"/> that was loaded if found; otherwise <c>null</c>.</returns>
        ILevelSave LoadLevel(long id);

        /// <summary>
        /// Saves the specified <see cref="Level"/> to the database.
        /// </summary>
        /// <param name="level"><see cref="Level"/> to save.</param>
        void SaveLevel(ILevelSave level);

        // TODO: Interacting with alliances as well.
    }
}
