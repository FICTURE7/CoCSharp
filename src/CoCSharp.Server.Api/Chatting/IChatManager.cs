using System;
using System.Collections.Generic;

namespace CoCSharp.Server.Api.Chatting
{
    /// <summary>
    /// Interface representing a chat manager.
    /// </summary>
    public interface IChatManager
    {
        /// <summary>
        /// Handles the specified chat message.
        /// </summary>
        /// <param name="client"><see cref="IClient"/> that sent the message.</param>
        /// <param name="message">Chat message to handle.</param>
        void Handle(IClient client, string message);

        /// <summary>
        /// Sends the specified chat message to the specified <see cref="IClient"/>.
        /// </summary>
        /// <param name="client"><see cref="IClient"/> to send the message to.</param>
        /// <param name="message">Chat message to send.</param>
        void SendChatMessage(IClient client, string message);

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> that iterates through the available command types.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that iterates through the available command types.</returns>
        IEnumerable<Type> GetCommandTypes();
    }
}
