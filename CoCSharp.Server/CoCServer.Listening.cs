using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public partial class CoCServer
    {
        // Total number of connection accepted.
        private int _totalConnection;
        private readonly Socket _listener;
        private readonly SocketAsyncEventArgsPool _acceptPool;

        // Returns a new SocketAsyncEventArgs object with Completed
        // set to AcceptOperationCompleted. 
        private SocketAsyncEventArgs CreateNewAcceptArgs()
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += AcceptOperationCompleted;
            return args;
        }

        private void StartListener()
        {
            const int PORT = 9339;

            var endpoint = new IPEndPoint(IPAddress.Any, PORT);
            try
            {
                _listener.Bind(endpoint);
                _listener.Listen(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: unable to start listener on {0}: {1}", endpoint, ex.Message);
                Environment.Exit(1);
            }

            StartAccept();
        }

        private void StartAccept()
        {
            try
            {
                var acceptArgs = _acceptPool.Pop();
                if (acceptArgs == null)
                    acceptArgs = CreateNewAcceptArgs();

                acceptArgs.AcceptSocket = null;
                while (!_listener.AcceptAsync(acceptArgs))
                {
                    ProcessAccept(acceptArgs);
                    acceptArgs.AcceptSocket = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: unable to start accept: {0}", ex.Message);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                ProcessBadAccept(args);
                return;
            }

            try
            {
                Interlocked.Increment(ref _totalConnection);
                var remoteSocket = args.AcceptSocket;
                var remoteEndPoint = args.AcceptSocket.RemoteEndPoint;
                Console.WriteLine("listener: accepted new remote connection: {0}", remoteEndPoint);

                // Start accepting new connection ASAP.
                ThreadPool.QueueUserWorkItem((object state) =>
                {
                    var client = new AvatarClient(this, remoteSocket, _settings);
                    Clients.Add(client);
                });

                args.AcceptSocket = null;
            }
            catch (Exception ex)
            {
                Log.Exception("unable to process accept: ", ex);
            }
        }

        private void ProcessBadAccept(SocketAsyncEventArgs args)
        {
            Console.WriteLine("listener: encountered bad accept");
            try
            {
                args.AcceptSocket.Close();
                args.AcceptSocket = null;
                //_acceptPool.Push(args);
            }
            catch (Exception ex)
            {
                Log.Exception("unable to close bad socket: ", ex);
            }
        }

        private void AcceptOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            ProcessAccept(args);
            StartAccept();
            _acceptPool.Push(args);
        }
    }
}
