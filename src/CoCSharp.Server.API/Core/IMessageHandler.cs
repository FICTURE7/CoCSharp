using CoCSharp.Network;

namespace CoCSharp.Server.API.Core
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
        /// Handles the specified <see cref="Message"/>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        void Handle(IClient client, Message message);
    }
}
