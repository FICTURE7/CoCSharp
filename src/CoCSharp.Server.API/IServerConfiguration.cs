namespace CoCSharp.Server.API
{
    /// <summary>
    /// Represents the configuration of an <see cref="IServer"/>.
    /// </summary>
    public interface IServerConfiguration
    {
        /// <summary>
        /// Gets the starting gems.
        /// </summary>
        int StartingGems { get; }

        /// <summary>
        /// Gets the starting gold.
        /// </summary>
        int StartingGold { get; }

        /// <summary>
        /// Gets the starting elixir.
        /// </summary>
        int StartingElixir { get; }

        /// <summary>
        /// Loads the <see cref="IServerConfiguration"/> at the specified path.
        /// </summary>
        /// <param name="path">Path pointing to the <see cref="IServerConfiguration"/>.</param>
        void Load(string path);

        /// <summary>
        /// Creates a new file representing <see cref="IServerConfiguration"/> values.
        /// </summary>
        /// <param name="path">Path at which to create the <see cref="IServerConfiguration"/>.</param>
        void Create(string path);
    }
}
