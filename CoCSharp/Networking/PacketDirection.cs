namespace CoCSharp.Networking
{
    /// <summary>
    /// Direction of the packet.
    /// </summary>
    public enum PacketDirection
    {
        /// <summary>
        /// To client.
        /// </summary>
        Client = 0x00,

        /// <summary>
        /// To server.
        /// </summary>
        Server = 0x01
    };
}
