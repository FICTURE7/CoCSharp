using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Exception that is thrown when there is an issue handling logic.
    /// </summary>
    public class LogicException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicException"/> class.
        /// </summary>
        public LogicException() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LogicException(string message) : base(message)
        {
            // Space
        }
    }
}
