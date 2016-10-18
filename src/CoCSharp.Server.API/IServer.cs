using CoCSharp.Data;
using CoCSharp.Logic;
using CoCSharp.Server.API.Core;

namespace CoCSharp.Server.API
{
    /// <summary>
    /// Represents a Clash of Clans server.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Gets the <see cref="Data.AssetManager"/> associated with the <see cref="IServer"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// This <see cref="Data.AssetManager"/> will be used to provide data to the <see cref="Level"/> that will
        /// be loaded.
        /// </remarks>
        AssetManager AssetManager { get; }

        /// <summary>
        /// Gets the <see cref="IDbManager"/> that will manage avatars.
        /// </summary>
        IDbManager DbManager { get; }

        /// <summary>
        /// Starts the <see cref="IServer"/> and start listening for incoming connections.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the <see cref="IServer"/> and disposes all resources used by it.
        /// </summary>
        void Close();
    }
}
