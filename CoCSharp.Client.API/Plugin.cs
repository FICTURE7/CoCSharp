namespace CoCSharp.Client.API
{
    public abstract class Plugin
    {
        public Plugin()
        {
            // Space
        }

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

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Author { get; }

        public virtual void OnUpdate() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnLoad() { }
        public virtual void OnUnload() { }
    }
}
