using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Client
{
    class RemoteServer
    {
        private Socket listener;
        private List<Socket> clients; 
        private IPEndPoint listeningEndPoint;
        private Thread RecvThread;
        public RemoteServer(IPEndPoint listeningEndPoint)
        {
            if (listeningEndPoint == null) throw new ArgumentNullException(nameof(listeningEndPoint));
            // #IHateAsyncNetworking
            this.listeningEndPoint = listeningEndPoint;
            listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            RecvThread = new Thread(StartAccept);
            RecvThread.Start();
        }

        public void Start()
        {
            RecvThread.Start();
        }

        public void Stop()
        {
            RecvThread.Abort();
        }

        ~RemoteServer()
        {
            RecvThread.Abort();
        }

        public void StartAccept()
        {
            while (true)
            {
                try
                {
                    Socket client = listener.Accept();
                    clients.Add(client);
                    new Thread(StartRecv).Start(client);
                }
                catch
                {
                    
                } 
            }
        }

        public void StartRecv(object socket)
        {
            Socket sock = socket as Socket;
            if (sock == null) throw new ArgumentNullException(nameof(sock));

            try
            {
                while (true)
                {
                    while (sock.Available < 1)
                    {
                        Thread.Sleep(10);
                    }
                    byte[] packet = new byte[sock.Available];
                    if (packet.Length > 2)
                    {

                    }
                    else
                    {

                    }
                }
            }
            catch
            {
                clients.Remove(sock);
                sock.Dispose();
            }
            
        }
    }
}
