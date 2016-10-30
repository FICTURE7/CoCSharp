using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using System;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class Client
    {
        public Client(Socket client, Socket server, NetworkManagerAsyncSettings settings)
        {
            Processor = new MessageProcessorNaClProxy(Crypto8.StandardKeyPair);
            ClientConnection = new NetworkManagerAsync(client, settings, Processor);
            ClientConnection.MessageReceived += ClientReceived;

            ServerConnection = new NetworkManagerAsync(server, settings, Processor);
            ServerConnection.MessageReceived += ServerReceived;
        }

        public MessageProcessorNaClProxy Processor { get; private set; }
        // Connection to the client.
        public NetworkManagerAsync ClientConnection { get; private set; }
        // Connection to the server.
        public NetworkManagerAsync ServerConnection { get; private set; }

        private void ClientReceived(object sender, MessageReceivedEventArgs e)
        {
            // C -> P
            Console.WriteLine("C -> P: {0}", e.Message.ID);

            var message = e.Message;
            var sendingBytes = e.Raw;
            if (Processor.State == 3)
            {
                var body = new byte[e.Plaintext.Length + CoCKeyPair.NonceLength * 2];
                Buffer.BlockCopy(Processor.SessionKey, 0, body, 0, CoCKeyPair.NonceLength);
                Buffer.BlockCopy(Processor.ClientNonce, 0, body, CoCKeyPair.NonceLength, CoCKeyPair.NonceLength);
                Buffer.BlockCopy(e.Plaintext, 0, body, CoCKeyPair.NonceLength * 2, e.Plaintext.Length);

                Processor.ServerCrypto.Encrypt(ref body);

                sendingBytes = new byte[body.Length + MessageHeader.Size + CoCKeyPair.KeyLength];
                Buffer.BlockCopy(e.Raw, 0, sendingBytes, 0, MessageHeader.Size);
                Buffer.BlockCopy(Processor.ServerCrypto.KeyPair.PublicKey, 0, sendingBytes, MessageHeader.Size, CoCKeyPair.KeyLength);
                Buffer.BlockCopy(body, 0, sendingBytes, MessageHeader.Size + CoCKeyPair.KeyLength, body.Length);
            }
            else if (Processor.State > 3)
            {
                // Could send the e.Raw back because we are using the same keys and nonces.
                // But lets reconstruct the packet because we're badasses.
                var body = (byte[])e.Plaintext.Clone();
                Processor.ServerCrypto.Encrypt(ref body);

                sendingBytes = new byte[body.Length + MessageHeader.Size];
                Buffer.BlockCopy(e.Raw, 0, sendingBytes, 0, MessageHeader.Size);
                Buffer.BlockCopy(body, 0, sendingBytes, MessageHeader.Size, body.Length);
            }

            // Forward data to the server.
            ServerConnection.Socket.Send(sendingBytes);
        }

        private void ServerReceived(object sender, MessageReceivedEventArgs e)
        {
            // P <- S
            Console.WriteLine("P <- S: {0}", e.Message.ID);

            var message = e.Message;
            var sendingBytes = e.Raw;

            if (Processor.State == 4)
            {
                var body = new byte[e.Plaintext.Length + CoCKeyPair.NonceLength + CoCKeyPair.KeyLength];
                Buffer.BlockCopy(Processor.ServerNonce, 0, body, 0, CoCKeyPair.NonceLength);
                Buffer.BlockCopy(Processor.ServerCrypto.SharedKey, 0, body, CoCKeyPair.NonceLength, CoCKeyPair.KeyLength);
                Buffer.BlockCopy(e.Plaintext, 0, body, CoCKeyPair.NonceLength + CoCKeyPair.KeyLength, e.Plaintext.Length);
                Processor.ClientCrypto.Encrypt(ref body);

                sendingBytes = new byte[body.Length + MessageHeader.Size];
                Buffer.BlockCopy(e.Raw, 0, sendingBytes, 0, MessageHeader.Size);
                Buffer.BlockCopy(body, 0, sendingBytes, MessageHeader.Size, body.Length);
            }
            else if (Processor.State > 4)
            {
                // Could send the e.Raw back because we are using the same keys and nonces.
                // But lets reconstruct the packet because we're badasses.
                var body = (byte[])e.Plaintext.Clone();
                Processor.ClientCrypto.Encrypt(ref body);

                sendingBytes = new byte[body.Length + MessageHeader.Size];
                Buffer.BlockCopy(e.Raw, 0, sendingBytes, 0, MessageHeader.Size);
                Buffer.BlockCopy(body, 0, sendingBytes, MessageHeader.Size, body.Length);
            }

            // Forward data to the client.
            ClientConnection.Socket.Send(sendingBytes);
        }
    }
}
