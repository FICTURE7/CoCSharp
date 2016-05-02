using System;
using System.Runtime.Serialization;

namespace CoCSharp.Network
{
    /// <summary>
    /// The exception that is thrown when there is an error with a 
    /// <see cref="Command"/>.
    /// </summary>
    [Serializable]
    public class CommandException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class.
        /// </summary>
        public CommandException() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class with
        /// the specified <see cref="Command"/> instance that caused the error.
        /// </summary>
        /// <param name="commandInstance">
        /// The <see cref="Command"/> instance that caused the error.
        /// </param>
        public CommandException(Command commandInstance)
        {
            CommandInstance = commandInstance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CommandException(string message) : base(message)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class with a 
        /// specified error message and <see cref="Message"/> instance that caused the error.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="commandInstance">
        /// The <see cref="Command"/> instance that caused the error.
        /// </param>
        public CommandException(string message, Command commandInstance) : base(message)
        {
            CommandInstance = commandInstance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class with a 
        /// specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified. 
        /// </param>
        public CommandException(string message, Exception innerException) : base(message, innerException)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected CommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // Space
        }

        /// <summary>
        /// Gets the <see cref="Command"/> instance that caused the error.
        /// </summary>
        public Command CommandInstance { get; protected set; }
    }
}
