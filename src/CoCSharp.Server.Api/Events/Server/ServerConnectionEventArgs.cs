using System;

namespace CoCSharp.Server.Api.Events.Server
{
    /// <summary>
    /// Event data for <see cref="IServer.ClientConnected"/>.
    /// </summary>
    public class ServerConnectionEventArgs : ServerEventArgs
    {
        enum ConnectionResult
        {
            Success,

            Failed,

            Banned
        };

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConnectionEventArgs"/> class with
        /// the specified <see cref="IServer"/> and <see cref="IClient"/>.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="client"></param>
        public ServerConnectionEventArgs(IServer server, IClient client) : base(server)
        {
            _client = client;
        }
        #endregion

        #region Fields & Properties
        private readonly IClient _client;

        /// <summary>
        /// Gets the <see cref="IClient"/> which got connected.
        /// </summary>
        public IClient Client => _client;
        #endregion
    }
}
