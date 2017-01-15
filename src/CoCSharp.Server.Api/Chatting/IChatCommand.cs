namespace CoCSharp.Server.Api.Chatting
{
    /// <summary>
    /// Interface representing a chat command.
    /// </summary>
    public interface IChatCommand
    {
        /// <summary>
        /// Gets the name of the <see cref="IChatCommand"/>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the alias of the <see cref="IChatCommand"/>.
        /// </summary>
        string[] Alias { get; }

        /// <summary>
        /// Gets the description of the <see cref="IChatCommand"/>.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the <see cref="IChatManager"/> that is handling this <see cref="IChatCommand"/>.
        /// </summary>
        IChatManager Manager { get; }

        /// <summary>
        /// Executes the <see cref="IChatCommand"/> instance on the specified <see cref="IClient"/> with the specified arguments.
        /// </summary>
        /// <param name="client"><see cref="IClient"/> on which to execute this <see cref="IChatCommand"/> instance.</param>
        /// <param name="args">Arguments to use.</param>
        void Execute(IClient client, params string[] args);
    }
}
