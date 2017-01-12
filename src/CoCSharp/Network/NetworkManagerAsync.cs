using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Network
{
    /// <summary>
    /// Implements methods to send and receive <see cref="Message"/> from <see cref="System.Net.Sockets.Socket"/> asynchronously
    /// using the <see cref="SocketAsyncEventArgs"/> model.
    /// </summary>
    public class NetworkManagerAsync : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class with the specified <see cref="System.Net.Sockets.Socket"/>
        /// from which messages will be received and sent; and <see cref="MessageProcessor"/> that is going to process incoming and outgoing messages.
        /// </summary>
        ///
        /// <remarks>
        /// The <see cref="NetworkManagerAsync"/> will use the <see cref="NetworkManagerAsyncSettings.DefaultSettings"/> to acquire a 
        /// receive and send pool.
        /// </remarks>
        /// 
        /// <param name="socket">
        ///     <see cref="System.Net.Sockets.Socket"/> instance from which messages will be received and sent.
        /// </param>
        /// 
        /// <param name="processor">
        ///     <see cref="MessageProcessor"/> instance that is going to process incoming and outgoing messages.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="socket"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="processor"/> is null.</exception>
        public NetworkManagerAsync(Socket socket, MessageProcessor processor)
            : this(socket, NetworkManagerAsyncSettings.DefaultSettings, processor)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class with the specified <see cref="System.Net.Sockets.Socket"/>
        /// from which messages will be received and sent; <see cref="NetworkManagerAsyncSettings"/> and 
        /// <see cref="MessageProcessor"/> that is going to process incoming and outgoing messages.
        /// </summary>
        /// 
        /// <param name="socket">
        ///     <see cref="System.Net.Sockets.Socket"/> instance from which messages will be received and sent.
        /// </param>
        /// 
        /// <param name="settings">
        ///     <see cref="NetworkManagerAsyncSettings"/> instance that is going to be used.
        /// </param>
        /// 
        /// <param name="processor">
        ///     <see cref="MessageProcessor"/> instance that is going to process incoming and outgoing messages.
        /// </param>
        public NetworkManagerAsync(Socket socket, NetworkManagerAsyncSettings settings, MessageProcessor processor)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            _processor = processor;
            _socket = socket;
            _settings = settings;
            _stats = new NetworkManagerAsyncStatistics();

            _settingsStats = Settings.Statistics;
            _receivePool = Settings._receivePool;
            _sendPool = Settings._sendPool;

            var args = _receivePool.Pop();
            if (args == null)
            {
                Debug.WriteLine("Creating new receive operation because pool was empty.");

                args = new SocketAsyncEventArgs();
                args.SetBuffer(new byte[settings.BufferSize], 0, settings.BufferSize);
                MessageReceiveToken.Create(args);
            }

            StartReceive(args);
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// The event raised when a <see cref="Message"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        /// <summary>
        /// The event raised when a <see cref="Message"/> is sent.
        /// </summary>
        public event EventHandler<MessageSentEventArgs> MessageSent;
        /// <summary>
        /// The event raised when <see cref="Socket"/> socket got disconnected.
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        // To figure out if have been disposed or not.
        private int _disposed;
        // Socket which the NetworkManagerAsync wraps.
        private readonly Socket _socket;
        // Processor that will process incoming encrypted byte arrays,
        // and process outgoing message objects.
        private readonly MessageProcessor _processor;

        // Pool of SocketAsyncEventArgs that is going to be used to
        // receive incoming data.
        private readonly SocketAsyncEventArgsPool _receivePool;
        // Pool of SocketAsyncEventArgs that is going to be used to
        // send data.
        private readonly SocketAsyncEventArgsPool _sendPool;

        // Stats for the current NetworkManagerAsync.
        private readonly NetworkManagerAsyncStatistics _stats;

        // Settings that NetworkManagerAsync will use, _receivePool and _sendPool
        // comes from _settings._receivePool and _settings._sendPool.
        private readonly NetworkManagerAsyncSettings _settings;
        // Stats for the NetworkManagerAsyncSettings which the instance
        // was initialize with.
        private readonly NetworkManagerAsyncStatistics _settingsStats;

#if DEBUG
        // ID of the NetworkManagerAysnc to debug stuff.
        private readonly string _ident;
#endif

        /// <summary>
        /// Gets the <see cref="System.Net.Sockets.Socket"/> that is used to send and receive
        /// data.
        /// </summary>
        public Socket Socket => _socket;

        /// <summary>
        /// Gets the <see cref="NetworkManagerAsyncSettings"/> being used the
        /// current <see cref="NetworkManagerAsync"/>.
        /// </summary>
        public NetworkManagerAsyncSettings Settings => _settings;

        /// <summary>
        /// Gets the <see cref="NetworkManagerAsyncStatistics"/> associated with
        /// the current <see cref="NetworkManagerAsync"/>.
        /// </summary>
        public NetworkManagerAsyncStatistics Statistics => _settingsStats;

        /// <summary>
        /// Gets the <see cref="MessageProcessor"/> that is going to process incoming
        /// and outgoing <see cref="Message"/> objects.
        /// </summary>
        public MessageProcessor Processor => _processor;
        #endregion

        #region Methods
        /// <summary>
        /// Sends the specified message using the <see cref="Socket"/> socket.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to send.</param>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="NetworkManagerAsync"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
        /// <exception cref="InvalidMessageException"><paramref name="message"/> length greater than <see cref="Message.MaxSize"/>.</exception>
        public void SendMessage(Message message)
        {
            if (Thread.VolatileRead(ref _disposed) == 1)
                throw new ObjectDisposedException(null, "Cannot SendMessage because the NetworkManagerAsync object was disposed.");
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var writerStream = new MemoryStream();
            using (var writer = new MessageWriter(writerStream))
            {
                var chiper = _processor.ProcessOutgoing(message);
                if (chiper == null)
                    throw new InvalidOperationException("MessageProcessor failed to process outgoing message.");

                var len = BitConverter.GetBytes(chiper.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(len);

                writer.Write(message.Id);
                writer.Write(len, 1, 3); // message length
                writer.Write(message.Version);
                writer.Write(chiper); // encrypted body

                var messageData = writerStream.ToArray();
                if (messageData.Length > Message.MaxSize)
                    throw new InvalidMessageException("Message size too large.", message);

                var args = _sendPool.Pop();
                if (args == null)
                {
                    Debug.WriteLine("Creating new send operation because pool was empty.");

                    args = new SocketAsyncEventArgs();
                    args.SetBuffer(new byte[65535], 0, 65535);
                    MessageSendToken.Create(args);
                }

                var token = (MessageSendToken)args.UserToken;
                token.Message = message;
                token.Id = message.Id;
                token.Length = chiper.Length;
                token.Version = message.Version;
                token.Body = messageData;

                StartSend(args);
            }
        }

        private void StartSend(SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.OperationAborted || Thread.VolatileRead(ref _disposed) == 1)
            {
                _sendPool.Push(args);
                return;
            }

            args.Completed += AsyncOperationCompleted;
            var token = (MessageSendToken)args.UserToken;

            // If still we have bytes to send.
            if (token.SendRemaining > 0)
            {
                // If message larger than buffer size.
                if (token.SendRemaining > Settings.BufferSize)
                {
                    Buffer.BlockCopy(token.Body, token.SendOffset, args.Buffer, args.Offset, Settings.BufferSize);
                }
                // Else resize buffer count.
                else
                {
                    Buffer.BlockCopy(token.Body, token.SendOffset, args.Buffer, args.Offset, token.SendRemaining);
                    args.SetBuffer(args.Offset, token.SendRemaining);
                }
            }

            if (!Socket.SendAsync(args))
                ProcessSend(args);
        }

        private void ProcessSend(SocketAsyncEventArgs args)
        {
            var bytesToProcess = args.BytesTransferred;
            var token = (MessageSendToken)args.UserToken;

            if (bytesToProcess == 0 || args.SocketError != SocketError.Success)
            {
                _sendPool.Push(args);
                OnDisconnected(new DisconnectedEventArgs(args.SocketError));
                return;
            }

            Interlocked.Add(ref Statistics._totalSent, args.BytesTransferred);
            Interlocked.Add(ref _settingsStats._totalSent, args.BytesTransferred);

            token.SendOffset += bytesToProcess;
            // If we still have bytes to send.
            if (token.SendRemaining > 0)
            {
                StartSend(args);
            }
            // Else reset and push back the SocketAsyncEventArgs.
            else
            {
                OnMessageSent(new MessageSentEventArgs
                {
                    Message = token.Message,
                });

                token.Reset();

                // Just in case.
                args.SetBuffer(args.Offset, Settings.BufferSize);
                _sendPool.Push(args);
            }
        }

        private void StartReceive(SocketAsyncEventArgs args)
        {
            // If we've been disposed, we can exit gently out of
            // of the function.
            if (Thread.VolatileRead(ref _disposed) == 1)
            {
                // Push the SocketAsyncEventArgs back to the pool.
                _receivePool.Push(args);
                return;
            }

            args.Completed += AsyncOperationCompleted;
            if (!Socket.ReceiveAsync(args))
                AsyncOperationCompleted(Socket, args);
        }

        private void ProcessReceive(SocketAsyncEventArgs args)
        {
            var bytesToProcess = args.BytesTransferred;
            var token = (MessageReceiveToken)args.UserToken;
            var messagesArgs = new List<MessageReceivedEventArgs>(2);

            Interlocked.Add(ref Statistics._totalReceived, args.BytesTransferred);
            Interlocked.Add(ref _settingsStats._totalReceived, args.BytesTransferred);

            while (bytesToProcess != 0)
            {
                // Appends the incoming bytes to a buffer.

                // If we don't have the complete header yet.
                // Copy header from buffer.
                if (Message.HeaderSize != token.HeaderOffset)
                {
                    // If we don't have the complete header in a single receive operation.
                    // Copy what we have from the buffer and reuse the same SocketAsyncEventArgs
                    // object to receive the rest.
                    if (bytesToProcess < token.HeaderRemaining)
                    {
                        //Console.WriteLine("Reusing args: {0}", token.TokenID);

                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Header, token.HeaderOffset, bytesToProcess);
                        token.Offset += bytesToProcess;
                        token.HeaderOffset += bytesToProcess;
                        bytesToProcess = 0;
                        token.Offset = args.Offset;

                        StartReceive(args);
                        return;
                    }
                    else
                    {
                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Header, token.HeaderOffset, token.HeaderRemaining);
                        bytesToProcess -= token.HeaderRemaining;
                        token.Offset += token.HeaderRemaining;
                        token.HeaderOffset += token.HeaderRemaining;

                        // Process the header, so we know how large the message is etc.
                        ProcessReceiveToken(token);
                    }
                }

                // If we don't have the complete body yet.
                // Copy body from buffer
                if (token.Length != token.BodyOffset)
                {
                    // If we don't have the complete body in a single receive operation.
                    // Copy what we have from the buffer.
                    if (bytesToProcess < token.BodyRemaining)
                    {
                        //Console.WriteLine("Reusing args: {0}", token.TokenID);

                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Body, token.BodyOffset, bytesToProcess);
                        token.Offset += bytesToProcess;
                        token.BodyOffset += bytesToProcess;
                        bytesToProcess = 0;
                        token.Offset = args.Offset;

                        StartReceive(args);
                        return;
                    }
                    else
                    {
                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Body, token.BodyOffset, token.BodyRemaining);
                        bytesToProcess -= token.BodyRemaining;
                        token.Offset += token.BodyRemaining;
                        token.BodyOffset += token.BodyRemaining;
                    }
                }

                // Full message data, that is message header including message body.
                var messageData = new byte[token.Length + Message.HeaderSize];
                // Copies 7 bytes long header.
                Buffer.BlockCopy(token.Header, 0, messageData, 0, Message.HeaderSize);
                // Copies message body starting at offset 7.
                Buffer.BlockCopy(token.Body, 0, messageData, Message.HeaderSize, token.Length);

                // Begins processing message bytes.
                var exception = (Exception)null;
                var message = (Message)null;
                var plaintext = (byte[])null;
                try
                {
                    var header = new MessageHeader(token.Id, token.Length, token.Version);
                    message = _processor.ProcessIncoming(header, token.Body, ref plaintext);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                var msgArgs = new MessageReceivedEventArgs()
                {
                    Message = message,
                    Raw = messageData,
                    Plaintext = plaintext,
                    Exception = exception
                };
                messagesArgs.Add(msgArgs);
                token.Reset();
            }

            token.Offset = args.Offset;
            _receivePool.Push(args);

            StartReceive(_receivePool.Pop());

            for (int i = 0; i < messagesArgs.Count; i++)
            {
#if DEBUG
                var sw = Stopwatch.StartNew();
#endif
                OnMessageReceived(messagesArgs[i]);

#if DEBUG
                sw.Stop();
                Debug.WriteLine("Done raising event for {0} in {1}ms.", messagesArgs[i].Message.GetType(), sw.Elapsed.TotalMilliseconds);
#endif
            }
        }

        private void ProcessReceiveToken(MessageReceiveToken token)
        {
            token.Id = (ushort)((token.Header[0] << 8) | (token.Header[1]));
            token.Length = (token.Header[2] << 16) | (token.Header[3] << 8) | (token.Header[4]);
            token.Version = (ushort)((token.Header[5] << 8) | (token.Header[6]));

            // Allocate a byte array of the same length as the message length.
            // Where all the remaining bytes are copied to.
            token.Body = new byte[token.Length];
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            const int TIMEOUT = 5000;

            args.Completed -= AsyncOperationCompleted;
            if (Settings._concurrentOps.WaitOne(TIMEOUT))
            {
                // Return the SocketAsyncEventArgs object back to its corresponding pool if
                // the NetworkManagerAsync was disconnected purposely or disposed.
                if (args.SocketError == SocketError.OperationAborted || Thread.VolatileRead(ref _disposed) == 1)
                {
                    switch (args.LastOperation)
                    {
                        case SocketAsyncOperation.Receive:
                            _receivePool.Push(args);
                            break;

                        case SocketAsyncOperation.Send:
                            _sendPool.Push(args);
                            break;
                    }

                    Settings._concurrentOps.Release();
                    return;
                }

                if (args.BytesTransferred == 0 || args.SocketError != SocketError.Success)
                {
                    switch (args.LastOperation)
                    {
                        case SocketAsyncOperation.Receive:
                            _receivePool.Push(args);
                            break;

                        case SocketAsyncOperation.Send:
                            _sendPool.Push(args);
                            break;
                    }

                    Settings._concurrentOps.Release();
                    OnDisconnected(new DisconnectedEventArgs(args.SocketError));
                    return;
                }

                switch (args.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(args);
                        break;

                    case SocketAsyncOperation.Send:
                        ProcessSend(args);
                        break;

                    default:
                        throw new InvalidOperationException("IMPOSSIBRU! Unexpected SocketAsyncOperation: " + args.LastOperation);
                }

                Settings._concurrentOps.Release();
            }
            else
            {
                Console.WriteLine("Semaphore not responding in time.");
                Debug.WriteLine("Semaphore not responding in time.");

                //TODO: Might want drop connection and release the semaphore.
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all unmanaged resources and optionally releases managed resources
        /// used by the current instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Make sure we haven't been disposed already.
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                if (disposing)
                {
                    try { Socket.Shutdown(SocketShutdown.Both); }
                    catch { /* Swallow. */}

                    try { Socket.Close(); }
                    catch { /* Swallow. */}
                }
            }
        }

        /// <summary>
        /// Use this method to trigger the <see cref="MessageReceived"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            Interlocked.Increment(ref Statistics._totalMessagesReceived);
            Interlocked.Increment(ref _settingsStats._totalMessagesReceived);

            if (MessageReceived != null && Thread.VolatileRead(ref _disposed) == 0)
                MessageReceived(this, e);
        }

        /// <summary>
        /// Use this method to trigger the <see cref="MessageSent"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnMessageSent(MessageSentEventArgs e)
        {
            Interlocked.Increment(ref Statistics._totalMessagesSent);
            Interlocked.Increment(ref _settingsStats._totalMessagesSent);

            if (MessageSent != null && Thread.VolatileRead(ref _disposed) == 0)
                MessageSent(this, e);
        }

        /// <summary>
        /// Use this method to trigger the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }
        #endregion
    }
}
