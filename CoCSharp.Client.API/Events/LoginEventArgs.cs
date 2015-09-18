using CoCSharp.Networking.Packets;
using System;

namespace CoCSharp.Client.API.Events
{
    public class LoginEventArgs : EventArgs
    {
        public LoginEventArgs(LoginSuccessPacket packet)
        {
            Packet = packet;
        }

        public LoginSuccessPacket Packet { get; set; }
    }
}
