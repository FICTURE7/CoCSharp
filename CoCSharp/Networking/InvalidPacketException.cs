using System;
using System.Runtime.Serialization;

namespace CoCSharp.Networking
{
    /// <summary>
    /// 
    /// </summary>
    public class InvalidPacketException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public InvalidPacketException()
            : base()
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
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
