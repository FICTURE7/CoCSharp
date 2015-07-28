using CoCSharp.Networking;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public class CoCServer
    {
        public CoCServer()
        {
            this.Clients = new List<CoCRemoteClient>();
            this.AcceptAsyncEventPool = new SocketAsyncEventArgsPool(100);
        }

        public Socket Listener { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public List<CoCRemoteClient> Clients { get; set; }

        private bool ShuttingDown { get; set; }
        private SocketAsyncEventArgsPool AcceptAsyncEventPool { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            ShuttingDown = false;
            EndPoint = endPoint;
            Listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(endPoint);
            Listener.Listen(100);

            AcceptSocketAsync();
        }

        public void Stop()
        {
            if (Listener != null)
                Listener.Close();
        }

        private void AcceptSocketAsync()
        {
            // make 9 of the SocketAsyncEventArgs listen
            while (AcceptAsyncEventPool.Count > 1 && !ShuttingDown) 
            {
                var args = AcceptAsyncEventPool.Pop();
                args.Completed += OperationCompleted;

                if (!Listener.AcceptAsync(args)) // if sync
                    OperationCompleted(Listener, args);
            }
        }

        private void OperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OperationCompleted;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    Clients.Add(new CoCRemoteClient(this, e.AcceptSocket));

                    e.AcceptSocket = null;
                    AcceptAsyncEventPool.Push(e); // reuse the async stuff
                    break;
            }

            AcceptSocketAsync();
        }
    }
}
