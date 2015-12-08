namespace CoCSharp.Networking
{
    /// <summary>
    /// Represents a Clash of Clans message.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Represents the size in bytes of the header of a Clash of Clans
        /// message. This field is constant.
        /// </summary>
        public const int HeaderSize = 7;

        /// <summary>
        /// Represents the maximum size in bytes of a Clash of Clans message. This field is
        /// constant
        /// </summary>
        public const int MaxSize = 16777215; // (2^24) - 1

        /// <summary>
        /// Represents the minimum size in bytes of a Clash of Clans message. This field is
        /// constant.
        /// </summary>
        public const int MinSize = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="Message"/>.
        /// </summary>
        public abstract ushort ID { get; }

        /// <summary>
        /// Gets the version of the <see cref="Message"/>.
        /// </summary>
        public virtual ushort Version { get { return 0; } }

        /// <summary>
        /// Reads the <see cref="Message"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="Message"/>.
        /// </param>
        public abstract void ReadMessage(MessageReader reader);

        /// <summary>
        /// Writes the <see cref="Message"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Message"/>.
        /// </param>
        public abstract void WriteMessage(MessageWriter writer);
    }
}
