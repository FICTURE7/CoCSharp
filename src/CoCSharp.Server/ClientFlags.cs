using System;

namespace CoCSharp.Server
{
    // Some flags to describe the state of an AvatarClient.
    [Flags]
    public enum ClientFlags : int
    {
        Disconnected = 0,

        Connected = 2,

        Loaded = 4,

        LoggedIn = 8,
    }
}
