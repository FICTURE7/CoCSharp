using System;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Instructs the <see cref="CsvSerializer"/> to serialize the member as a
    /// <see cref="TimeSpan"/> struct with the specified column names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CsvTimeSpanAttribute : Attribute
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CsvTimeSpanAttribute"/> class.
        /// </summary>
        public CsvTimeSpanAttribute()
        {
            // Space
        }
    }
}
