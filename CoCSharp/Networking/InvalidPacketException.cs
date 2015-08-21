using System;
using System.Runtime.Serialization;

namespace CoCSharp.Networking
{
    /// <summary>
    /// The exception that is thrown when a Clash of Clans packet is invalid.
    /// </summary>
    public class InvalidPacketException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPacketException"/> class.
        /// </summary>
        public InvalidPacketException()
            : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPacketException"/> class with
        /// the specified message.
        /// </summary>
        /// <param name="message">The message thats describe the error.</param>
        public InvalidPacketException(string message)
            : base(message)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidPacketException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public InvalidPacketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Space
        }
    }
}
