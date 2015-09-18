using CoCSharp.Client.API.Events;
using CoCSharp.Data;
using CoCSharp.Logging;
using CoCSharp.Logic;
using CoCSharp.Networking;
using System;
using System.Net.Sockets;

namespace CoCSharp.Client.API
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICoCClient
    {
        /// <summary>
        /// Gets or sets the <see cref="Socket"/> which is associated with this <see cref="ICoCClient"/>.
        /// </summary>
        Socket Connection { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Logic.Village"/> which is associated with this <see cref="ICoCClient"/>.
        /// </summary>
        Village Home { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Logic.Avatar"/> which is  associated with this <see cref="ICoCClient"/>.
        /// </summary>
        Avatar Avatar { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Data.Fingerprint"/> associated with this <see cref="ICoCClient"/>.
        /// </summary>
        Fingerprint Fingerprint { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Logging.PacketLogger"/> associated with this <see cref="ICoCClient"/>.
        /// </summary>
        PacketLogger PacketLogger { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Networking.NetworkManagerAsync"/> associated with this <see cref="ICoCClient"/>.
        /// </summary>
        NetworkManagerAsync NetworkManager { get; set; }

        /// <summary>
        /// Sends the specified message to the global chat.
        /// </summary>
        /// <param name="message"></param>
        void SendChatMessage(string message);

        /// <summary>
        /// The event raised when a chat message is received.
        /// </summary>
        event EventHandler<ChatMessageEventArgs> ChatMessage;
        /// <summary>
        /// The event raised when logged in successfully.
        /// </summary>
        event EventHandler<LoginEventArgs> Login;
    }
}