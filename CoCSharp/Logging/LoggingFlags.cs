using System;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Controls the <see cref="LogBuilder"/> on how it logs objects
    /// and data.
    /// </summary>
    [Flags]
    public enum LoggingFlags
    {
        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log unknowns.
        /// </summary>
        Unknowns = 2,

        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log properties.
        /// </summary>
        Properties = 4,

        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log fields.
        /// </summary>
        Fields = 8,

        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to print the log in
        /// the console.
        /// </summary>
        Console = 16,

        /// <summary>
        /// Default <see cref="LoggingFlags"/> CoCSharp uses which instructs
        /// the <see cref="LogBuilder"/> to log both <see cref="Unknowns"/> and 
        /// <see cref="Fields"/> with <see cref="Console"/> set.
        /// </summary>
        Default = Unknowns | Fields | Console,

        /// <summary>
        /// Instructs the <see cref="LogBuilder"/> to log unknowns, properties,
        /// fields and print to console.
        /// </summary>
        All = Unknowns | Properties | Fields | Console
    };
}
