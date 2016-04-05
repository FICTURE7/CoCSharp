using CoCSharp.Networking;

namespace CoCSharp.Server
{
    public delegate void CommandHandler(CoCServer server, CoCRemoteClient client, Command command);
}