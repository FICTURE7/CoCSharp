using System;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Instruct the <see cref="CsvSerializer"/> to serialize the with the specified
    /// property name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CsvPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvPropertyAttribute"/> class with
        /// the specified property name.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        public CsvPropertyAttribute(string name)
        {
            PropertyName = name;
        }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName { get; set; }
    }
}
