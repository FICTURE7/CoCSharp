using System;

namespace CoCSharp.Server.Api.Events.Server
{
    /// <summary>
    /// Represents an <see cref="EventArgs"/> raised by the server.
    /// </summary>
    public abstract class ServerEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEventArgs"/> class with
        /// the specified <see cref="IServer"/>.
        /// </summary>
        /// <param name="server"></param>
        public ServerEventArgs(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
        }
        #endregion

        #region Fields & Properties
        private readonly IServer _server;

        /// <summary>
        /// Gets the <see cref="IServer"/> which fired the event.
        /// </summary>
        public IServer Server => _server;
        #endregion
    }
}
