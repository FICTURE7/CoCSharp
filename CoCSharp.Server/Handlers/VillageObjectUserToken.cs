namespace CoCSharp.Server.Handlers
{
    public class VillageObjectUserToken
    {
        // A way to figure which avatar to save on events.
        public VillageObjectUserToken(CoCServer server, CoCRemoteClient client)
        {
            Server = server;
            Client = client;
        }

        public CoCServer Server { get; set; }

        public CoCRemoteClient Client { get; set; }
    }
}
