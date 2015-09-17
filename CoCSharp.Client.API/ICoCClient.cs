using CoCSharp.Client.API.Events;
using CoCSharp.Data;
using CoCSharp.Logging;
using CoCSharp.Logic;
using CoCSharp.Networking;
using System;
using System.Net.Sockets;

namespace CoCSharp.Client.API
{
    public interface ICoCClient
    {
        Socket Connection { get; set; }
        Village Home { get; set; }
        Avatar Avatar { get; set; }
        Fingerprint Fingerprint { get; set; }
        PacketLogger PacketLogger { get; set; }
        NetworkManagerAsync NetworkManager { get; set; }

        event EventHandler<ChatMessageEventArgs> ChatMessage;
        event EventHandler<LoginEventArgs> Login;
    }
}