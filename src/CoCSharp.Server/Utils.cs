using CoCSharp.Network;
using System.Diagnostics;
using System.IO;

namespace CoCSharp.Server
{
    public class Utils
    {
        public static T ReadMessageAt<T>(string path) where T : Message, new()
        {
            var message = new T();
            var file = File.ReadAllBytes(path);
            var stream = new MemoryStream(file);
            using (var reader = new MessageReader(stream))
            {
                message.ReadMessage(reader);
            }

            return message;
        }
    }
}
