using CoCSharp.Network;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to provide
    /// information about the enemy's home.
    /// </summary>
    public class EnemyHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyHomeDataMessage"/> class.
        /// </summary>
        public EnemyHomeDataMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="EnemyHomeDataMessage"/>.
        /// </summary>
        public override ushort ID { get { return 0; } }

        /// <summary>
        /// Reads the <see cref="EnemyHomeDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="EnemyHomeDataMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="EnemyHomeDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="EnemyHomeDataMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            // Space
        }
    }
}
