using System;

namespace CoCSharp.Networking
{
    [Flags]
    public enum PacketExtractionFlags : byte
    {
        Header = 2,
        Body = 4,
        Remove = 8
    };
}
