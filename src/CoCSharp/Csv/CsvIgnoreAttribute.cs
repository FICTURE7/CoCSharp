using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Instructs the <see cref="CsvConvert"/> to not serialize the member. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CsvIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvIgnoreAttribute"/> class.
        /// </summary>
        public CsvIgnoreAttribute()
        {
            // Space
        }
    }
}
