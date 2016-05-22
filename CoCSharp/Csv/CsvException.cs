using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// The exception thrown when an error occurs during CSV serialization or deserialization.
    /// </summary>
    public class CsvException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvException"/> class.
        /// </summary>
        public CsvException()
            : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvException"/> class with
        /// the specified error message.
        /// </summary>
        /// <param name="message">Message of the <see cref="CsvException"/>.</param>
        public CsvException(string message)
            : base(message)
        {
            // Space
        }
    }
}
