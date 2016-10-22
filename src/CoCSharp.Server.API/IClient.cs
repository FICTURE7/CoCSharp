using CoCSharp.Logic;
using CoCSharp.Network;
using System;
using System.Net.Sockets;

namespace CoCSharp.Server.API
{
    /// <summary>
    /// Represents a remote client.
    /// </summary>
    public interface IClient : IDisposable
    {
        /// <summary>
        /// Event raised when <see cref="IClient"/> leaves the server.
        /// </summary>
        event EventHandler<DisconnectedEventArgs> Left;

        ///// <summary>
        ///// Event raised when <see cref="IClient"/> joins the server.
        ///// </summary>
        //event EventHandler<EventArgs> Joined;

        /// <summary>
        /// Gets the <see cref="IServer"/> which owns this <see cref="IClient"/>.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Gets the <see cref="Socket"/> to which the <see cref="IClient"/> is connected to.
        /// </summary>
        Socket Connection { get; }

        /// <summary>
        /// Gets the <see cref="Logic.Level"/> associated with the <see cref="IClient"/>.
        /// </summary>
        Level Level { get; }

        /// <summary>
        /// Sends the specified <see cref="Message"/> to the <see cref="IClient"/> using the <see cref="Connection"/> <see cref="Socket"/>.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to send.</param>
        void SendMessage(Message message);

        /// <summary>
        /// Disconnects <see cref="Connection"/>.
        /// </summary>
        void Disconnect();
    }
}
