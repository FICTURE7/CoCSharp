﻿using System;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Instructs the <see cref="CsvSerializer"/> to not serialize the member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CsvIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CsvIgnoreAttribute"/> class.
        /// </summary>
        public CsvIgnoreAttribute()
        {
            // Spaces
        }
    }
}