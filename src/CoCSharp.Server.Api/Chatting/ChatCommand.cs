namespace CoCSharp.Server.Api.Chatting
{
    /// <summary>
    /// Base class of a chat command.
    /// </summary>
    public abstract class ChatCommand : IChatCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatCommand"/> class.
        /// </summary>
        public ChatCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the name of the <see cref="ChatCommand"/>
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the <see cref="ChatCommand"/>.
        /// </summary>
        public virtual string Description => null;

        /// <summary>
        /// Gets the alias of the <see cref="ChatCommand"/>.
        /// </summary>
        public virtual string[] Alias => null;

        /// <summary>
        /// Gets or sets the <see cref="IChatManager"/> that is handling this <see cref="ChatCommand"/>.
        /// </summary>
        public IChatManager Manager { get; set; }

        /// <summary>
        /// Executes the <see cref="ChatCommand"/> instance on the specified <see cref="IClient"/> with the specified arguments.
        /// </summary>
        /// <param name="client"><see cref="IClient"/> on which to execute this <see cref="ChatCommand"/> instance.</param>
        /// <param name="args">Arguments to use.</param>
        public abstract void Execute(IClient client, params string[] args);
    }
}
