namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to 
    /// request for alliances that it is capable of joining.
    /// </summary>
    public class JoinableAllianceListRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListRequestMessage"/> class.
        /// </summary>
        public JoinableAllianceListRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="JoinableAllianceListRequestMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14303; } }

        /// <summary>
        /// Reads the <see cref="JoinableAllianceListRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="JoinableAllianceListRequestMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="JoinableAllianceListRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="JoinableAllianceListRequestMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            // Space
        }
    }
}
