using System;

namespace CoCSharp.Client.API
{
    /// <summary>
    /// The base class that every plugin should inherit from.
    /// </summary>
    public abstract class Plugin : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        public Plugin()
        {
            // Space
        }

        ~Plugin()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the main client class.
        /// </summary>
        public ICoCClient Client
        {
            get
            {
                if (!m_Loaded)
                    throw new PluginException("You can access the 'Client' property after 'OnLoad()' was called.");
                return m_Client;
            }
            set
            {
                if (!m_Loaded) // loaded only once by the PluginManager class.
                {
                    m_Client = value;
                    m_Loaded = true;
                }
                else
                    throw new PluginException("You can modify the 'Client' property.");
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
        public virtual void OnUpdate()
        {
            // Space
        }

        /// <summary>
        /// Called when the plugin is enabled.
        /// </summary>
        public virtual void OnEnable()
        {
            // Space
        }

        /// <summary>
        /// Called when the plugin is disabled.
        /// </summary>
        public virtual void OnDisable()
        {
            // Space
        }

        /// <summary>
        /// Called when the plugin is loaded.
        /// </summary>
        public virtual void OnLoad()
        {
            // Space
        }

        /// <summary>
        /// Called when the plugin is unloaded.
        /// </summary>
        public virtual void OnUnload()
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //TODO: Document
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //TODO: Document
        }
    }
}
