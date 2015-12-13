using CoCSharp.Networking;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            Console.Title = "CoC# Proxy";

            var proxy = new CoCProxy();
            proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            Console.ReadLine();
        }
    }
}
