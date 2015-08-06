using System;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// The exception thrown when an error occurs during CSV serialization or deserialization.
    /// </summary>
    public class CsvSerializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvSerializationException"/> class.
        /// </summary>
        public CsvSerializationException()
            : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvSerializationException"/> class with
        /// the specified message.
        /// </summary>
        /// <param name="message">Message of the <see cref="Exception"/>.</param>
        public CsvSerializationException(string message)
            : base(message)
        {
            // Space
        }
    }
}
