using System;
using System.Runtime.Serialization;

namespace CoCSharp.Network
{
    /// <summary>
    /// The exception that is thrown when a <see cref="Command"/> is invalid.
    /// </summary>
    [Serializable]
    public class InvalidCommandException : CommandException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class.
        /// </summary>
        public InvalidCommandException() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class with
        /// the specified <see cref="Command"/> instance that caused the error.
        /// </summary>
        /// <param name="commandInstance">
        /// The <see cref="Command"/> instance that caused the error.
        /// </param>
        public InvalidCommandException(Command commandInstance) : base(commandInstance)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class with a 
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidCommandException(string message) : base(message)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class with a 
        /// specified error message and <see cref="Command"/> instance that caused the error.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="commandInstance">
        /// The <see cref="Command"/> instance that caused the error.
        /// </param>
        public InvalidCommandException(string message, Command commandInstance) : base(message, commandInstance)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class with a 
        /// specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified. 
        /// </param>
        public InvalidCommandException(string message, Exception innerException) : base(message, innerException)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected InvalidCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // Space
        }
    }
}
