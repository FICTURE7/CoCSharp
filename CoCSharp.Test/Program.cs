using CoCSharp.Databases;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CoCSharp.Test
{
    public class Program
    {
        public static Socket Listener { get; set; }
        public static List<byte[]> Buffers { get; set; }
        public static List<Socket> Connections { get; set; }
        public static Stack<SocketAsyncEventArgs> AcceptAysncEventPool { get; set; }
        public static Stack<SocketAsyncEventArgs> ReceiveAsyncEventPool { get; set; }
        public static Stack<SocketAsyncEventArgs> SendAsyncEventPool { get; set; }

        public static void main(string[] args)
        {
            // testing stuff for async networking
            Buffers = new List<byte[]>();
            InitializeBuffers();

            AcceptAysncEventPool = new Stack<SocketAsyncEventArgs>();
            InitializeAcceptAysnc();

            ReceiveAsyncEventPool = new Stack<SocketAsyncEventArgs>();
            InitializeReceiveAsync();

            SendAsyncEventPool = new Stack<SocketAsyncEventArgs>();
            InitializeSendAsync();

            Connections = new List<Socket>();
            Listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            Listener.Listen(100);

            while (true)
            {
                if (AcceptAysncEventPool.Count > 0)
                {
                    var acceptEvent = AcceptAysncEventPool.Pop();
                    var willRaiseEvent = Listener.AcceptAsync(acceptEvent);
                    if (!willRaiseEvent)
                    {
                        HandleAcceptOperation(acceptEvent);
                    }
                }
                Thread.Sleep(1);
            }
        }

        public static void Main()
        {
            Database.DownloadDatabasesAysnc("databases", "2b63d564d20bfa2c86d988b1e72848f1c2fa3095");
            //var webClient = new WebClient();
            //var str = webClient.DownloadString("http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/asdga");
            Console.ReadLine();
        }

        public static void InitializeAcceptAysnc()
        {
            for (int i = 0; i < 10; i++)
            {
                var acceptAsyncEvent = new SocketAsyncEventArgs();
                acceptAsyncEvent.Completed += AysncOperationCompleted;
                AcceptAysncEventPool.Push(acceptAsyncEvent);
            }
        }

        public static void InitializeReceiveAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                var receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.Completed += AysncOperationCompleted;
                ReceiveAsyncEventPool.Push(receiveAsyncEvent);
            }
        }

        public static void InitializeSendAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                var sendAsyncEvent = new SocketAsyncEventArgs();
                sendAsyncEvent.Completed += AysncOperationCompleted;
                SendAsyncEventPool.Push(sendAsyncEvent);
            }
        }

        public static void InitializeBuffers()
        {
            for (int i = 0; i < 10; i++)
            {
                Buffers.Add(new byte[1024]);
            }
        }

        public static void AysncOperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("{0} operation completed!", e.LastOperation);
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    HandleAcceptOperation(e);
                    break;

                case SocketAsyncOperation.Receive:
                    HandleReceiveOperation(e);
                    break;

                case SocketAsyncOperation.Send:
                    HandleSendOperation(e);
                    break;
            }
        }

        public static void HandleAcceptOperation(SocketAsyncEventArgs acceptEvent)
        {
            Connections.Add(acceptEvent.AcceptSocket);

            var receiveEvent = ReceiveAsyncEventPool.Pop();
            receiveEvent.SetBuffer(Buffers[0], 0, 1024);
            receiveEvent.AcceptSocket = acceptEvent.AcceptSocket;
            receiveEvent.AcceptSocket.ReceiveAsync(receiveEvent);

            acceptEvent.AcceptSocket = null;
            AcceptAysncEventPool.Push(acceptEvent);
        }

        public static void HandleReceiveOperation(SocketAsyncEventArgs receiveEvent)
        {
            var text = Encoding.UTF8.GetString(receiveEvent.Buffer, 0, receiveEvent.BytesTransferred);
            Console.WriteLine("Received: {0}", text);

            var sendEvent = SendAsyncEventPool.Pop();
            sendEvent.SetBuffer(Buffers[1], 0, 1024);
            sendEvent.AcceptSocket = receiveEvent.AcceptSocket;
            sendEvent.AcceptSocket.SendAsync(sendEvent);

            ReceiveAsyncEventPool.Push(receiveEvent);
        }

        public static void HandleSendOperation(SocketAsyncEventArgs sendEvent)
        {
            SendAsyncEventPool.Push(sendEvent);
        }
    }
}
