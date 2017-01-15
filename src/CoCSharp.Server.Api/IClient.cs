using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Server.Api.Db;
using System;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server.Api
{
    /// <summary>
    /// Represents a remote client.
    /// </summary>
    public interface IClient : IDisposable
    {
        #region Fields & Properties
        /// <summary>
        /// Gets the <see cref="IServer"/> which owns this <see cref="IClient"/>.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Gets the <see cref="Socket"/> to which the <see cref="IClient"/> is connected to.
        /// </summary>
        Socket Connection { get; }

        /// <summary>
        /// Gets the local <see cref="EndPoint"/> of the <see cref="IClient"/>.
        /// </summary>
        EndPoint LocalEndPoint { get; }

        /// <summary>
        /// Gets the remote <see cref="EndPoint"/> of the <see cref="IClient"/>.
        /// </summary>
        EndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Gets the <see cref="Logic.Session"/> associated with th e<see cref="IClient"/>.
        /// </summary>
        Session Session { get; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> of when the last keep alive response was received.
        /// </summary>
        DateTime LastKeepAliveTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> of when the keep alive expires.
        /// </summary>
        DateTime KeepAliveExpireTime { get; set; }

        LevelSave Save { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Sends the specified <see cref="Message"/> to the <see cref="IClient"/> using the <see cref="Connection"/> <see cref="Socket"/>.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to send.</param>
        void SendMessage(Message message);
        #endregion
    }
}
