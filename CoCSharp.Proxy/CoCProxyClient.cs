using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class CoCProxyClient
    {
        public CoCProxyClient(Socket client, Socket server, NetworkManagerAsyncSettings settings)
        {
            // Initiated first because message receive triggers too quickly sometimes.
            var crypto = new Crypto8(MessageDirection.Server);
            // Configure Crypto with Supercell's public key.
            // This is step 6. according to clugh's Protocol wiki page.
            crypto.UpdateSharedKey(Crypto8.SupercellPublicKey);

            // Client connection is initiated with standard keys because we are acting as the server.
            ClientConnection = new NetworkManagerAsync(client, settings);
            ClientConnection.MessageReceived += ClientReceived;

            // Server connection is initiated with generated keys because we are acting as the client.
            ServerConnection = new NetworkManagerAsync(server, settings, crypto);
            ServerConnection.MessageReceived += ServerReceived;

            var publicKeyS = Utils.BytesToString(ClientConnection.Crypto.KeyPair.PublicKey);
            var privateKeyS = Utils.BytesToString(ClientConnection.Crypto.KeyPair.PrivateKey);
            Console.WriteLine("Acting as server with standard \n\tpublic-key: {0} \n\tprivate-key: {1}", publicKeyS, privateKeyS);

            var publicKeyC = Utils.BytesToString(ServerConnection.Crypto.KeyPair.PublicKey);
            var privateKeyC = Utils.BytesToString(ServerConnection.Crypto.KeyPair.PrivateKey);
            Console.WriteLine("Acting as client with generated \n\tpublic-key: {0} \n\tprivate-key: {1}", publicKeyC, privateKeyC);

            _logger = new MessageLogger();
        }

        // Connection to the client.
        public NetworkManagerAsync ClientConnection { get; private set; }
        // Connection to the server.
        public NetworkManagerAsync ServerConnection { get; private set; }

        private readonly MessageLogger _logger;
        private byte[] _snonce;
        private byte[] _rnonce;

        private void ClientReceived(object sender, MessageReceivedEventArgs e)
        {
            // C -> P -> S
            _logger.Log(e.Message);

            //Console.WriteLine("[S < C] => ID:{0} Name:{1}", e.Message.ID, e.Message.GetType().Name);
            if (!e.MessageFullyRead)
                Console.WriteLine("        => Did not fully read.");
            if (e.Exception != null)
                Console.WriteLine("        => Warning: Exception occurred during reading: {0}", e.Exception.Message);

            var message = e.Message;
            var messageBytes = (byte[])null;
            if (message is SessionRequestMessage)
                messageBytes = e.MessageData;
            else if (message is LoginRequestMessage)
            {
                var lrMessage = e.Message as LoginRequestMessage;
                var rpkStr = Utils.BytesToString(lrMessage.PublicKey);
                var opkStr = Utils.BytesToString(ServerConnection.Crypto.KeyPair.PublicKey);

                Console.WriteLine("        => Decrypted LoginRequestMessage with pk {0}", rpkStr);
                _snonce = (byte[])lrMessage.Nonce.Clone();
                messageBytes = new byte[e.MessageData.Length];

                var body = (byte[])e.MessageBody.Clone();
                ServerConnection.Crypto.Encrypt(ref body);

                Console.WriteLine("        => Encrypted LoginRequestMessage with pk {0}", opkStr);
                Buffer.BlockCopy(e.MessageData, 0, messageBytes, 0, Message.HeaderSize); // Copies the header.
                Buffer.BlockCopy(ServerConnection.Crypto.KeyPair.PublicKey, 0, messageBytes, Message.HeaderSize, CoCKeyPair.KeyLength); // Copies our generated public key.
                Buffer.BlockCopy(body, 0, messageBytes, Message.HeaderSize + CoCKeyPair.KeyLength, body.Length); // Copies the body.

                ServerConnection.Crypto.UpdateNonce(_snonce, UpdateNonceType.Blake);
                ServerConnection.Crypto.UpdateNonce(_snonce, UpdateNonceType.Encrypt); // set _snonce for crypto to use for later encryption
            }
            else
            {
                messageBytes = new byte[e.MessageData.Length];

                var body = (byte[])e.MessageBody.Clone();
                ServerConnection.Crypto.Encrypt(ref body);

                Buffer.BlockCopy(e.MessageData, 0, messageBytes, 0, Message.HeaderSize); // header
                Buffer.BlockCopy(body, 0, messageBytes, Message.HeaderSize, body.Length); // body
            }

            File.WriteAllBytes("messages\\[C2S] " + DateTime.Now.ToString("hh-mm-ss.fff") + " " + e.Message.ID, e.MessageBody);
            ServerConnection.Connection.Send(messageBytes);
        }

        private void ServerReceived(object sender, MessageReceivedEventArgs e)
        {
            // C <- P <- S

            _logger.Log(e.Message);

            //Console.WriteLine("[S > C] => ID:{0} Name:{1}", e.Message.ID, e.Message.GetType().Name);
            if (!e.MessageFullyRead)
                Console.WriteLine("        => Warning: Did not fully read message.");
            if (e.Exception != null)
                Console.WriteLine("        => Warning: Exception occurred during reading: {0}", e.Exception.Message);


            var message = e.Message;
            var messageBytes = (byte[])null;
            if (message is SessionSuccessMessage)
                messageBytes = e.MessageData;
            else if (message is LoginSuccessMessage)
            {
                var lsMessage = e.Message as LoginSuccessMessage;
                _rnonce = (byte[])lsMessage.Nonce.Clone();
                messageBytes = new byte[e.MessageData.Length];

                var body = (byte[])e.MessageBody.Clone();
                ClientConnection.Crypto.Encrypt(ref body);

                Buffer.BlockCopy(e.MessageData, 0, messageBytes, 0, Message.HeaderSize); // header
                Buffer.BlockCopy(body, 0, messageBytes, Message.HeaderSize, body.Length); // body

                ClientConnection.Crypto.UpdateNonce(_rnonce, UpdateNonceType.Encrypt);
                ClientConnection.Crypto.UpdateSharedKey(lsMessage.PublicKey); // 'k'
            }
            else
            {
                try
                {
                    if (message is OwnHomeDataMessage)
                    {
                        var ohdMessage = message as OwnHomeDataMessage;
                        //File.WriteAllText("villages\\" + DateTime.Now.ToString("hh-mm-ss.fff") + " ownhomedata.json",
                        //                  ohdMessage.OwnVillageData.Home.DeserializedJson);
                    }
                }
                catch { }
                messageBytes = new byte[e.MessageData.Length];

                var body = e.MessageBody;
                ClientConnection.Crypto.Encrypt(ref body);

                Buffer.BlockCopy(e.MessageData, 0, messageBytes, 0, Message.HeaderSize); // header
                Buffer.BlockCopy(body, 0, messageBytes, Message.HeaderSize, body.Length); // body
            }

            File.WriteAllBytes("messages\\[C2S] " + DateTime.Now.ToString("hh-mm-ss.fff") + " " + e.Message.ID, e.MessageBody);
            ClientConnection.Connection.Send(messageBytes);
        }
    }
}
