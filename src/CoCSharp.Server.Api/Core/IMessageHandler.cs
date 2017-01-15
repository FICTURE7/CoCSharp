using CoCSharp.Network;
using CoCSharp.Server.Api.Chatting;
using System.Threading.Tasks;

namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Object that is going to handle incoming <see cref="Message"/>.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Gets the <see cref="IServer"/> which owns the <see cref="IMessageHandler"/>.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Gets the <see cref="IChatManager"/> which is going to handle chat messages.
        /// </summary>
        IChatManager ChatManager { get; }

        /// <summary>
        /// Handles the specified <see cref="Message"/> asynchronously.
        /// </summary>
        /// <param name="client"><see cref="IClient"/> that sent the message.</param>
        /// <param name="message"><see cref="Message"/> that <paramref name="client"/> sent.</param>
        /// <returns></returns>
        Task HandleAsync(IClient client, Message message);
    }
}
