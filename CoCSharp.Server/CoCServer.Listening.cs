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
                Log.Exception("unable to start listener on " + endpoint, ex);
                Environment.Exit(1);
            }

            StartAccept(null);
        }

        private void StartAccept(SocketAsyncEventArgs args)
        {
            try
            {
                if (args == null)
                {
                    args = _acceptPool.Pop();
                    if (args == null)
                        args = CreateNewAcceptArgs();
                }

                args.AcceptSocket = null;
                if (!_listener.AcceptAsync(args))
                    ProcessAccept(args);
            }
            catch (Exception ex)
            {
                Log.Exception("***unable to start accept", ex);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            try
            {
                Interlocked.Increment(ref _totalConnection);
                var remoteSocket = args.AcceptSocket;
                var remoteEndPoint = args.AcceptSocket.RemoteEndPoint;
                Console.WriteLine("listener: accepted new remote connection: {0}", remoteEndPoint);
                args.AcceptSocket = null;

                // Start accepting new connection ASAP.
                ThreadPool.QueueUserWorkItem((object state) =>
                {
                    var client = new AvatarClient(this, remoteSocket, _settings);
                    Clients.Add(client);
                });
            }
            catch (Exception ex)
            {
                Log.Exception("***unable to process accept: ", ex);
            }
            finally
            {
                StartAccept(args);
            }
        }

        private void ProcessBadAccept(SocketAsyncEventArgs args)
        {
            Log.Warning("listener: ***encountered bad accept");
            try
            {
                args.AcceptSocket.Close();
                args.AcceptSocket = null;
                _acceptPool.Push(args);
            }
            catch (Exception ex)
            {
                Log.Exception("***unable to close bad socket: ", ex);
            }
            finally
            {
                StartAccept(null);
            }
        }

        private void AcceptOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                ProcessBadAccept(args);
                return;
            }

            ProcessAccept(args);
        }
    }
}
