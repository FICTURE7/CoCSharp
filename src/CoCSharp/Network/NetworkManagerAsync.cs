// Define VERBOSE only we're debugging so that
// we can access the _ident field.
#if DEBUG
#define VERBOSE
#endif

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="processor">
        ///     <see cref="MessageProcessor"/> instance that is going to process incoming and outgoing messages.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="socket"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="processor"/> is null.
        /// </exception>
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
#if DEBUG
            _ident = Guid.NewGuid().ToString();
#endif

            _settingsStats = Settings.Statistics;

            _receivePool = Settings._receivePool;
            _sendPool = Settings._sendPool;

            _usedIncomingBuffers = new Pool<byte[]>(4);
            _incomingBuffer = new BufferStream(_settings, _usedIncomingBuffers);

            // Take a SocketAsyncEventArgs object from the pool or create new instance
            // if empty.
            var args = _settings.GetArgs();

            // Set SocketAsyncEventArgs to write directly 
            // into the stream buffer.
            args.SetBuffer(_incomingBuffer._buf, 0, _settings.BufferSize);

            // Begin receiving on the args.
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
        // receive incoming data. OBSOLETE.
        private readonly SocketAsyncEventArgsPool _receivePool;
        // Pool of SocketAsyncEventArgs that is going to be used to
        // send data. OBSOLETE.
        private readonly SocketAsyncEventArgsPool _sendPool;

        // Stats for the current NetworkManagerAsync.
        private readonly NetworkManagerAsyncStatistics _stats;

        // Settings that NetworkManagerAsync will use, _receivePool and _sendPool
        // comes from _settings._receivePool and _settings._sendPool.
        private readonly NetworkManagerAsyncSettings _settings;
        // Stats for the NetworkManagerAsyncSettings which the instance
        // was initialize with.
        private readonly NetworkManagerAsyncStatistics _settingsStats;

        // Receive buffer stream.
        private readonly BufferStream _incomingBuffer;
        // Send buffer stream.
        private readonly BufferStream _outgoingBuffer;

        // Pool of buffers we used.
        private readonly Pool<byte[]> _usedIncomingBuffers;

        // The results of the TaskCompletionSources are set in there respective
        // "event raiser" methods.

        // TaskCompletionSource for ReceiveMessageAsync.
        private TaskCompletionSource<Message> _receiveTask;
        // TaskCompletionSource for SendMessageAsync.
        private TaskCompletionSource<object> _sendTask;

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
        /// Sends the specified <see cref="Message"/> using the <see cref="Socket"/> socket asynchronously using a <see cref="Task"/>
        /// to represent the send operation.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to send.</param>
        /// 
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="NetworkManagerAsync"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
        /// 
        /// <remarks>
        /// The <see cref="SendMessageAsync(Message)"/> method is simply a wrapper around <see cref="SendMessage(Message)"/> and
        /// <see cref="MessageSent"/> using <see cref="TaskCompletionSource{TResult}"/>.
        /// </remarks>
        public Task SendMessageAsync(Message message)
        {
            if (Thread.VolatileRead(ref _disposed) == 1)
                throw new ObjectDisposedException(null, "Cannot SendMessage because the NetworkManagerAsync object was disposed.");
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _sendTask = new TaskCompletionSource<object>();
            SendMessage(message);
            return _sendTask.Task;
        }

        /// <summary>
        /// Receives a <see cref="Message"/> from the <see cref="Socket"/> socket asynchronously using a <see cref="Task"/> to represent
        /// the receive operation.
        /// </summary>
        /// 
        /// <returns><see cref="Message"/> that was received.</returns>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="NetworkManagerAsync"/> is disposed.</exception>
        /// 
        /// <remarks>
        /// The <see cref="ReceiveMessageAsync"/> method is simply a wrapper around <see cref="MessageReceived"/> using <see cref="TaskCompletionSource{TResult}"/>.
        /// </remarks>
        public Task<Message> ReceiveMessageAsync()
        {
            if (Thread.VolatileRead(ref _disposed) == 1)
                throw new ObjectDisposedException(null, "Cannot SendMessage because the NetworkManagerAsync object was disposed.");

            _receiveTask = new TaskCompletionSource<Message>();
            return _receiveTask.Task;
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/> using the <see cref="Socket"/> socket asynchronously.
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
                var cipher = _processor.ProcessOutgoing(message);
                if (cipher == null)
                    throw new InvalidOperationException("MessageProcessor failed to process outgoing message.");

                // Make sure the message isn't too large.
                if (cipher.Length > Message.MaxSize)
                    throw new InvalidMessageException("Message size too large.", message);

                var messageLength = BitConverter.GetBytes(cipher.Length);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(messageLength);

                // Header.
                writer.Write(message.Id);
                writer.Write(messageLength, 1, 3); // Message length
                writer.Write(message.Version);
                // Body encrypted.
                writer.Write(cipher);

                var messageData = writerStream.ToArray();

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
                token.Length = cipher.Length;
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
#if VERBOSE
            Debug.WriteLine("Starting receive on {0}.", args: _ident);
#endif
            Debug.Assert(args != null);
            Debug.Assert(args.Buffer != null);

            // If we've been disposed, we can exit gently out of
            // of the function.
            if (Thread.VolatileRead(ref _disposed) == 1)
            {
                // Push the SocketAsyncEventArgs back to the pool since
                // we're done with it.
                _settings.Recyle(args);
                return;
            }

            // Register our event handler to handle the event args.
            args.Completed += AsyncOperationCompleted;
            if (!Socket.ReceiveAsync(args))
                AsyncOperationCompleted(Socket, args);
        }

        private void ProcessReceive(SocketAsyncEventArgs args)
        {
#if VERBOSE
            Debug.WriteLine("Processing receive on {0}.", args: _ident);
#endif
            Debug.Assert(args != null);
            Debug.Assert(args.Buffer != null);
            Debug.Assert(args.LastOperation == SocketAsyncOperation.Receive);
            Debug.Assert(args.Count <= _settings.BufferSize);
            Debug.Assert(args.Buffer.Length == _settings.BufferSize);

            // If we've filled the buffer completely
            // we take a new buffer and write to that new buffer instead.
            if (args.BytesTransferred == args.Count)
            {
                // Take a new buffer from the NetworkManagerAsyncSettings.
                var newBuffer = _settings.GetBuffer();
                _usedIncomingBuffers.Push(newBuffer);

                // Set the SocketAsyncEventArgs to write directly 
                // into the stream buffer.
                args.SetBuffer(newBuffer, 0, _settings.BufferSize);
            }
            else
            {
                // Set the SocketAsyncEventArgs to append to the
                // the current buffer instead of overwriting.

                // Move the write(offset) pointer of the args to continue
                // appending to the buffer.
                var offset = args.BytesTransferred;
                var count = _settings.BufferSize - offset;

                args.SetBuffer(offset, count);
            }

            // Keep track of how much data with appended to the incoming buffer.
            _incomingBuffer._len += args.BytesTransferred;

            // Try to process new messages.
            var messageArgs = default(MessageReceivedEventArgs);
            do
            {
                // Get next incoming message from the buffers.
                // Returns null if we don't have enough data usually.
                messageArgs = ProcessNextIncomingMessage();
#if VERBOSE
                if (messageArgs != null)
                {
                    if (messageArgs.Message != null)
                        Debug.WriteLine("MessageProcessor processed {0}", args: messageArgs.Message.GetType().Name);
                    else
                        Debug.WriteLine("MessageProcessor returned null.");
                }
#endif

                // Raise the event handler and let the user handle the message.
                if (messageArgs != null)
                    OnMessageReceived(messageArgs);
            } while (messageArgs != null);

            // Start receiving more after we're done processing the messages on
            // the incoming buffer.
            StartReceive(args);
        }

        private MessageReceivedEventArgs ProcessNextIncomingMessage()
        {
            // If we don't have the header yet we can exit early.
            if (_incomingBuffer.Length - _incomingBuffer.Position < MessageHeader.Size)
            {
#if VERBOSE
                Debug.WriteLine("Not enough data to process message header, returning null.");
#endif
                return null;
            }

            // Read the header from the incoming buffer stream.
            var headerBytes = new byte[MessageHeader.Size];
            var count = _incomingBuffer.Read(headerBytes, 0, MessageHeader.Size);

            Debug.Assert(count == MessageHeader.Size);

            // Process header bytes and get the ID, length and version
            // of the message.
            var header = ProcessHeader(headerBytes);
            // Make sure we have enough bytes before processing.
            if (_incomingBuffer.Length - _incomingBuffer.Position < header.Length)
            {
#if VERBOSE
                Debug.WriteLine("Not enough data to process message body, moving back 7 bytes, returning null.");
#endif
                Debug.Assert(_incomingBuffer.Position >= Message.HeaderSize);
                // Move the Position backwards to restore the stream to its original position if
                // we don't have enough bytes to read from the buffer stream to read the complete
                // message. nice
                _incomingBuffer.Seek(-MessageHeader.Size, SeekOrigin.Current);
                return null;
            }

            var raw = default(byte[]);
            var plaintext = default(byte[]);
            var messageArgs = new MessageReceivedEventArgs();
            try
            {
                // Pass the incoming buffer stream to the processor to handle encryption
                // and all that good stuff.
                var message = _processor.ProcessIncoming(header, _incomingBuffer, ref raw, ref plaintext);
                messageArgs.Message = message;
                messageArgs.Raw = raw;
                messageArgs.Plaintext = plaintext;
            }
            catch (Exception ex)
            {
                messageArgs.Exception = ex;
            }

            return messageArgs;
        }

        private MessageHeader ProcessHeader(byte[] buffer)
        {
            Debug.Assert(buffer.Length >= MessageHeader.Size);

            var id = (ushort)((buffer[0] << 8) | (buffer[1]));
            var length = (buffer[2] << 16) | (buffer[3] << 8) | (buffer[4]);
            var version = (ushort)((buffer[5] << 8) | (buffer[6]));

            const int CLIENT_MIN = 1000;
            const int SERVER_MAX = 3000;
            Debug.Assert(id >= CLIENT_MIN || id < SERVER_MAX);
            Debug.Assert(length < Message.MaxSize);

            return new MessageHeader(id, length, version);
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            // Semaphore timeout.
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

                    //TODO: Wait before any work on the _usedIncomingBuffers is done before recycling.
#if !DEBUG
                    // Recycled all buffers we used.
                    for (int i = 0; i < _usedIncomingBuffers.Count; i++)
                    {
                        var buffer = _usedIncomingBuffers.Pop();
                        _settings.Recyle(buffer);
                    }
#endif
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

            // If the _receiveTask is not null, we set its result.
            _receiveTask?.SetResult(e.Message);
            _receiveTask = null;

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

            _sendTask?.SetResult(e.Message);
            _sendTask = null;

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
