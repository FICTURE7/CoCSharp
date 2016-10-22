using CoCSharp.Data;
using CoCSharp.Network;
using CoCSharp.Server.API.Core;
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
        /// <summary>
        /// Event raised when the server has started.
        /// </summary>
        event EventHandler<EventArgs> Started;

        /// <summary>
        /// Gets the <see cref="Data.AssetManager"/> associated with the <see cref="IServer"/>.
        /// </summary>
        AssetManager Assets { get; }

        /// <summary>
        /// Gets the <see cref="Logging.Logger"/> associated with the <see cref="IServer"/>.
        /// </summary>
        Logger Logger { get; }
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
        /// Starts the <see cref="IServer"/> and start listening for incoming connections.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the <see cref="IServer"/> and disposes all resources used by it.
        /// </summary>
        void Close();
        /// <summary>
        /// Processes the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to process.</param>
        void ProcessMessage(Message message);
    }
}
