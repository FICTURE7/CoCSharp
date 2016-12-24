namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to tell
    /// it that it has left its alliance.
    /// </summary>
    public class LeaveAllianceMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveAllianceMessage"/> class.
        /// </summary>
        public LeaveAllianceMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="LeaveAllianceMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14308; } }

        /// <summary>
        /// Reads the <see cref="LeaveAllianceMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LeaveAllianceMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="LeaveAllianceMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LeaveAllianceMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            // Space
        }
    }
}
