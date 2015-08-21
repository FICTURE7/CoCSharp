using System;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Instructs the <see cref="CsvSerializer"/> to not serialize the member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CsvIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CsvIgnoreAttribute"/> class.
        /// </summary>
        public CsvIgnoreAttribute()
        {
            // Space
        }
    }
}
