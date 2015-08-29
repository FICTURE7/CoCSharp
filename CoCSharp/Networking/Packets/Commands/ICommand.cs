namespace CoCSharp.Networking.Packets.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        int ID { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        void ReadCommand(PacketReader reader);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        void WriteCommand(PacketWriter writer);
    }
}
