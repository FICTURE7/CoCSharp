using CoCSharp.Data;
using CoCSharp.Data.Csv;
using CoCSharp.Logic;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# Server";

            Server = new CoCServer();
            Server.Start(new IPEndPoint(IPAddress.Any, 9339));
            Thread.Sleep(-1);
        }

        public static void m()
        {
            var listSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listSocket.Bind(new IPEndPoint(IPAddress.Any, 9339));
            listSocket.Listen(100);
            var socket = listSocket.Accept();
            var buffer = new byte[65535];
            socket.Receive(buffer);
        }
    }
}
