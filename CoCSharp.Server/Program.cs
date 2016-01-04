using CoCSharp.Networking;
using CoCSharp.Networking.Messages;
using System;
using System.IO;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void Main(string[] args)
        {
            Server = new CoCServer();
            Server.Start();
            Thread.Sleep(Timeout.Infinite);
        }

        public static void m()
        {
            var message = new OwnHomeDataMessage();
            var reader = new MessageReader(new MemoryStream(File.ReadAllBytes("ownhome")));
            message.ReadMessage(reader);

            var writer = new MessageWriter(new MemoryStream());
            message.WriteMessage(writer);
            File.WriteAllBytes("reownhome", ((MemoryStream)writer.BaseStream).ToArray());
        }
    }
}
