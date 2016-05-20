using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Instruct the <see cref="CsvConvert"/> to serialize a property with the specified
    /// alias name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CsvAliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvAliasAttribute"/> class with
        /// the specified alias name.
        /// </summary>
        /// <param name="name">The alias name of the field.</param>
        public CsvAliasAttribute(string name)
        {
            Alias = name;
        }

        /// <summary>
        /// Gets or sets alias name.
        /// </summary>
        public string Alias { get; set; }
    }
}
