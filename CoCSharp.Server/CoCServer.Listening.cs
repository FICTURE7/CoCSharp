using CoCSharp.Server.Core;
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

        private void StartListener()
        {
            const int PORT = 9339;

            var endpoint = new IPEndPoint(IPAddress.Any, PORT);
            try
            {
                _listener.Bind(endpoint);
                _listener.Listen(100);

                StartAccept();
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: unable to start listener on {0}: {1}", endpoint, ex.Message);
                Environment.Exit(1);
            }
        }

        private void StartAccept()
        {
            var acceptArgs = _acceptPool.Pop();
            if (acceptArgs == null)
            {
                acceptArgs = CreateNewAcceptArgs();
            } 

            if (!_listener.AcceptAsync(acceptArgs))
                ProcessAccept(acceptArgs);
        }
        
        // Returns a new SocketAsyncEventArgs object with Completed
        // set to AcceptOperationCompleted. 
        private SocketAsyncEventArgs CreateNewAcceptArgs()
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += AcceptOperationCompleted;
            return args;
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                // Start accepting new connection ASAP.
                StartAccept();
                ProcessBadAccept(args);
                return;
            }

            // Start accepting new connection ASAP.
            StartAccept();

            var remoteEndPoint = args.AcceptSocket.RemoteEndPoint;
            // FancyConsole.WriteLine(LogFormats.Listener_Connected, args.AcceptSocket.RemoteEndPoint);
            Console.WriteLine("listener: accepted new remote connection: {0}", remoteEndPoint);

            Interlocked.Increment(ref _totalConnection);
            var client = new AvatarClient(this, args.AcceptSocket, _settings);
            Clients.Add(client);

            //Console.Title = "CoC# - Server: " + Clients.Count;

            args.AcceptSocket = null;
            _acceptPool.Push(args);
        }

        private void ProcessBadAccept(SocketAsyncEventArgs args)
        {
            Console.WriteLine("listener: encountered bad accept");
            try
            {
                args.AcceptSocket.Close();
                args.AcceptSocket = null;
                _acceptPool.Push(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: unable to close bad socket: {0}", ex.Message);
            }
        }

        private void AcceptOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            ProcessAccept(args);
        }
    }
}
