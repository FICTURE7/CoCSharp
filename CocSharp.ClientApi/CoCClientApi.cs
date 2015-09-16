namespace CoCSharp.PluginApi
{
    public interface IPlugin // interface required due to how the plugin system works
    {
        string Name { get; set; }
        string Desc { get; set; }
        string Author { get; set; }
        //TODO: List<string> Dependencies { get; set; }

        //Livecycle:
        void PreStart();
        void OnStart();
        void PostStart();
        void Update(); // Called every 100 ms
        void OnCmdRecv(string cmd, string[] param);
        void PreStop();
        void OnStop();
        void PostStop();
    }
}
