namespace CoCSharp.Client.API
{
    /// <summary>
    /// The base class that every plugin should inherit from.
    /// </summary>
    public abstract class Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin()
        {
            // Space
        }

        /// <summary>
        /// Gets the main client class.
        /// </summary>
        public ICoCClient Client
        {
            get
            {
                if (!m_Loaded)
                    throw new PluginException("OnLoad() was not called.");
                return m_Client;
            }
            set
            {
                if (!m_Loaded)
                {
                    m_Client = value;
                    m_Loaded = true;
                }
            }
        }
        private ICoCClient m_Client = null;
        private bool m_Loaded = false; // changes to true when Client is set

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        /// Called every 100ms.
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// Called when the plugin is enabled.
        /// </summary>
        public virtual void OnEnable() { }
        /// <summary>
        /// Called when the plugin is disabled.
        /// </summary>
        public virtual void OnDisable() { }
        /// <summary>
        /// Called when the plugin is loaded.
        /// </summary>
        public virtual void OnLoad() { }
        /// <summary>
        /// Called when the plugin is unloaded.
        /// </summary>
        public virtual void OnUnload() { }
    }
}
