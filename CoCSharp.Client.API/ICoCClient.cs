using CoCSharp.Client.API.Events;
using CoCSharp.Data;
using CoCSharp.Logic;
using CoCSharp.Networking.Packets;
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
        /// Sends the specified message to the global chat.
        /// </summary>
        /// <param name="message"></param>
        void SendChatMessage(string message);

        /// <summary>
        /// Registers the specified <see cref="PacketHandler"/> with the specified <see cref="IPacket"/>
        /// to this <see cref="ICoCClient"/>.
        /// </summary>
        /// <param name="packet">The <see cref="IPacket"/>.</param>
        /// <param name="handler">The <see cref="PacketHandler"/> delegate used to handle the specified
        /// <see cref="IPacket"/>.</param>
        void RegisterPacketHandler(IPacket packet, PacketHandler handler);

        /// <summary>
        /// Sends the specified <see cref="IPacket"/> to the server.
        /// </summary>
        /// <param name="packet">The <see cref="IPacket"/> to send.</param>
        void SendPacket(IPacket packet);

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