using CoCSharp.Networking;
using System;
using System.IO;

namespace CoCSharp.Proxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CoC# Proxy";

            var lel = new MemoryStream();
            var writer = new MessageWriter();

            var instance = MessageFactory.Create(0);

            Console.ReadLine();
        }
    }
}
