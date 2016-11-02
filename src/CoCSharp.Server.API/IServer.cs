using CoCSharp.Data;
using CoCSharp.Network;
using CoCSharp.Server.API.Core;
using CoCSharp.Server.API.Events.Server;
using CoCSharp.Server.API.Logging;
using System;
using System.Collections.Generic;

namespace CoCSharp.Server.API
{
    /// <summary>
    /// Represents a Clash of Clans server.
    /// </summary>
    public interface IServer : IDisposable
    {
        #region Fields & Properties
        /// <summary>
        /// Event raised when the server has started.
        /// </summary>
        event EventHandler<EventArgs> Started;

        /// <summary>
        /// Event raised when the server accepts a new connection.
        /// </summary>
        event EventHandler<ServerConnectionEventArgs> ClientConnected;

        /// <summary>
        /// Gets the <see cref="AssetManager"/> associated with the <see cref="IServer"/>.
        /// </summary>
        AssetManager Assets { get; }

        /// <summary>
        /// Gets the <see cref="Logging.Log"/> associated with the <see cref="IServer"/>.
        /// </summary>
        Log Log { get; }

        /// <summary>
        /// Gets the <see cref="IDbManager"/> that will manage avatars.
        /// </summary>
        IDbManager Db { get; }

        /// <summary>
        /// Gets the <see cref="ICollection{T}"/> of <see cref="IClient"/> connected
        /// to the <see cref="IServer"/>.
        /// </summary>
        ICollection<IClient> Clients { get; }

        /// <summary>
        /// Gets the <see cref="IServerConfiguration"/> the <see cref="IServer"/> uses.
        /// </summary>
        IServerConfiguration Configuration { get; }

        /// <summary>
        /// Gets the <see cref="IMessageHandler"/> that handles incoming <see cref="Message"/>.
        /// </summary>
        IMessageHandler Handler { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the <see cref="IServer"/> and start listening for incoming connections.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the <see cref="IServer"/> and disposes all resources used by it.
        /// </summary>
        void Close();
        #endregion
    }
}
