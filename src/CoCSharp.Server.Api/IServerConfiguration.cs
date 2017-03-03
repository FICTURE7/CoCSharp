namespace CoCSharp.Server.Api
{
    /// <summary>
    /// Represents the configuration of an <see cref="IServer"/>.
    /// </summary>
    public interface IServerConfiguration
    {
        /// <summary>
        /// Gets the value of the specified config name.
        /// </summary>
        /// <param name="configName"></param>
        /// <returns>Value of the specified config name.</returns>
        string this[string configName] { get; }

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
        /// Gets a value indicating whether or not to synchronize the assets
        /// on start up.
        /// </summary>
        bool SynchronizeAssets { get; }

        /// <summary>
        /// Gets the content url.
        /// </summary>
        string ContentUrl { get; }

        /// <summary>
        /// Gets the master hash.
        /// </summary>
        string MasterHash { get; }

        /// <summary>
        /// Gets the MySQL host.
        /// </summary>
        string MySqlHost { get; }

        /// <summary>
        /// Gets the MySQL port.
        /// </summary>
        int MySqlPort { get; }

        /// <summary>
        /// Gets the MySQL user.
        /// </summary>
        string MySqlUser { get; }

        /// <summary>
        /// Gets the MySQL password.
        /// </summary>
        string MySqlPassword { get; }

        /// <summary>
        /// Gets the starting village JSON.
        /// </summary>
        string StartingVillage { get; }

        /// <summary>
        /// Loads the <see cref="IServerConfiguration"/> at the specified path and returns
        /// <c>true</c> if loaded completely; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="path">Path pointing to the <see cref="IServerConfiguration"/>.</param>
        /// <returns><c>true</c> if loaded completely; otherwise, <c>false</c>.</returns>
        /// 
        /// <remarks>
        /// The returning value will determine whether or not to overwrite the save with the current values.
        /// </remarks>
        bool Load(string path);

        /// <summary>
        /// Saves a new file representing <see cref="IServerConfiguration"/> values.
        /// </summary>
        /// <param name="path">Path at which to save the <see cref="IServerConfiguration"/>.</param>
        void Save(string path);
    }
}
